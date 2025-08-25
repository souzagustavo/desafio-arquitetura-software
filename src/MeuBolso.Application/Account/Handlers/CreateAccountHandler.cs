using ErrorOr;
using MeuBolso.Application.Common.Handlers;
using MeuBolso.Domain.Account;

namespace MeuBolso.Application.Account.Handlers
{
    public record CreateAccountRequest(EAccountType Type);

    public record CreateAccountResponse(Guid Id);

    public interface ICreateAccountHandler : IHandler
    {
        Task<ErrorOr<CreateAccountResponse>> HandleAsync(
            Guid userId,
            CreateAccountRequest request,
            CancellationToken cancellationToken);
    }

    public class CreateAccountHandler : ICreateAccountHandler
    {
        private readonly IAccountRepository _accountRepository;

        public CreateAccountHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<ErrorOr<CreateAccountResponse>> HandleAsync(Guid userId, CreateAccountRequest request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetByTypeAsync(request.Type, cancellationToken);

            if (account is not null)
                return Error.Conflict("Account.Duplicate", "An account with this type already exists.");

            var mapper = new AccountMapper();
            var entity = mapper.ToAccountEntity(request);

            await _accountRepository.AddAsync(entity, cancellationToken);

            return new CreateAccountResponse(entity.Id);
        }
    }
}
