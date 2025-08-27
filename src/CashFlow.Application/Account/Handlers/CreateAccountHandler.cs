using CashFlow.Application.Common.Handlers;
using CashFlow.Application.Common.Interfaces;
using ErrorOr;

namespace CashFlow.Application.Account.Handlers
{
    public record CreateAccountRequest(string Name, decimal InitialBalance = 0);

    public record CreatedAccountResponse(Guid Id);

    public interface ICreateAccountHandler : IHandler
    {
        Task<ErrorOr<CreatedAccountResponse>> HandleAsync(
            Guid userId,
            CreateAccountRequest request,
            CancellationToken cancellationToken);
    }

    public class CreateAccountHandler : ICreateAccountHandler
    {
        private readonly ICashFlowDbContext _dbContext;

        public CreateAccountHandler(ICashFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ErrorOr<CreatedAccountResponse>> HandleAsync(Guid userId, CreateAccountRequest request, CancellationToken cancellationToken)
        {
            var mapper = new AccountMapper();
            var entity = mapper.ToAccountEntity(request);

            entity.IdentityUserId = userId;

            await _dbContext.Accounts.AddAsync(entity, cancellationToken);
            await _dbContext.AccountBalances
                .AddAsync(new() {
                    Account = entity,
                    CurrentTotal = request.InitialBalance
                }, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new CreatedAccountResponse(entity.Id);
        }
    }
}
