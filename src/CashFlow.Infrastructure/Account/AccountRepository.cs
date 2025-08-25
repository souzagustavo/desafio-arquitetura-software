using CashFlow.Application.Account;
using CashFlow.Domain.Account;
using CashFlow.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.Account
{
    public class AccountRepository : IAccountRepository
    {
        private readonly CashFlowDbContext _dbContext;

        public AccountRepository(CashFlowDbContext dbContext)
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
