using MeuBolso.Application.Account;
using MeuBolso.Domain.Account;
using MeuBolso.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MeuBolso.Infrastructure.Account
{
    public class AccountRepository : IAccountRepository
    {
        private readonly MeuBolsoDbContext _dbContext;

        public AccountRepository(MeuBolsoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(AccountEntity entity, CancellationToken cancellationToken)
        {
            await _dbContext.Accounts.AddAsync(entity, cancellationToken);
            await _dbContext.AccountBalances.AddAsync(new() { Account = entity }, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<AccountEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Accounts.FindAsync(id, cancellationToken);
        }

        public async Task<AccountEntity?> GetByTypeAsync(EAccountType type, CancellationToken cancellationToken)
        {
            return await _dbContext.Accounts.FirstOrDefaultAsync(a => a.Type == type, cancellationToken);
        }
    }
}
