using MeuBolso.Domain.Account;

namespace MeuBolso.Application.Account
{
    public interface IAccountRepository
    {
        Task AddAsync(AccountEntity entity, CancellationToken cancellationToken);
        Task<AccountEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<AccountEntity?> GetByTypeAsync(EAccountType type, CancellationToken cancellationToken);
    }
}
