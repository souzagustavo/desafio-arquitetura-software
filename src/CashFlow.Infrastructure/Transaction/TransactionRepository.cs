using CashFlow.Application.Common;
using CashFlow.Application.Transactions.Handlers;
using CashFlow.Domain.Transactions;
using CashFlow.Infrastructure.Common.Persistence;

namespace CashFlow.Infrastructure.Transaction
{
    public class TransactionRepository : ITransationsRepository
    {
        private readonly CashFlowDbContext _dbContext;

        public TransactionRepository(CashFlowDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(TransactionEntity entity, CancellationToken cancellationToken)
        {
            await _dbContext.Transactions.AddAsync(entity, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<TransactionEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Transactions.FindAsync(id, cancellationToken);
        }

        public async Task<PagedResult<TransactionEntity>> GetPagedAsync(
            GetPagedTransactionsQuery filter,            
            CancellationToken cancellationToken = default)
        {
            _dbContext.

                _cashFlowDbContext.Stores
            .Include(s => s.Transactions)
            .Where(s => s.IdentityUserId == userId)
            .SelectMany(s => s.Transactions)
            .FirstOrDefaultAsync(t => t.Id == id);

            return new PagedResult<TransactionEntity>
            {
                Items = items,
                CurrentPage = filter.Page,
                PageSize = filter.PageSize,
                TotalItems = totalItems,
                TotalPages = totalPages
            };
        }
    }
}
