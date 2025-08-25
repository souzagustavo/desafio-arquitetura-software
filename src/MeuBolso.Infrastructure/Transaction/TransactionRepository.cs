using MeuBolso.Application.Transactions;
using MeuBolso.Domain.Transactions;
using MeuBolso.Infrastructure.Common.Persistence;

namespace MeuBolso.Infrastructure.Transaction
{
    public class TransactionRepository : ITransationsRepository
    {
        private readonly MeuBolsoDbContext _dbContext;

        public TransactionRepository(MeuBolsoDbContext dbContext)
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
