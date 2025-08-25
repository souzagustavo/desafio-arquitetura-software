using CashFlow.Application.Store;
using CashFlow.Domain.Store;
using CashFlow.Infrastructure.Common.Persistence;

namespace CashFlow.Infrastructure.Store
{
    public class StoreRepository : IStoreRepository
    {
        private readonly CashFlowDbContext _dbContext;

        public StoreRepository(CashFlowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(StoreEntity entity, CancellationToken cancellationToken)
        {
            await _dbContext.Stores.AddAsync(entity, cancellationToken);
            await _dbContext.StoreBalances.AddAsync(new() { Store = entity }, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<StoreEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Stores.FindAsync(id, cancellationToken);
        }
    }
}
