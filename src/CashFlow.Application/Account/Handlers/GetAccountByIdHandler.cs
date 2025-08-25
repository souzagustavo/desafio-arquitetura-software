using ErrorOr;
using CashFlow.Application.Common.Handlers;
using CashFlow.Domain.Account;

namespace CashFlow.Application.Account.Handlers;

public record GetAccountResponse(Guid Id, EAccountType Type);

public interface IGetAccountByIdHandler : IHandler
{
    Task<ErrorOr<GetAccountResponse>> HandleAsync(
        Guid Id,
        CancellationToken cancellationToken);
}

public class GetAccountByIdHandler : IGetAccountByIdHandler
{
    private readonly IAccountRepository _accountRepository;
    public GetAccountByIdHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<ErrorOr<GetAccountResponse>> HandleAsync(Guid id, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(id, cancellationToken);

        if (account is null)
            return Error.NotFound(description: "Account not found.");

        var mapper = new AccountMapper();
        var response = mapper.ToGetAccountResponse(account);

        return response;
    }
}
