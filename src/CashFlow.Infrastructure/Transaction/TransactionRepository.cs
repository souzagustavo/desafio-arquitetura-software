using CashFlow.Application.Transactions;
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
    }
}
