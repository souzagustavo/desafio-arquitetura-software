using CashFlow.Domain.Account;

namespace CashFlow.Application.Account
{
    public interface IAccountRepository
    {
        Task AddAsync(AccountEntity entity, CancellationToken cancellationToken);
        Task<AccountEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<AccountEntity?> GetByTypeAsync(EAccountType type, CancellationToken cancellationToken);
    }
}
