using CashFlow.Domain.Store;

namespace CashFlow.Application.Store
{
    public interface IStoreRepository
    {
        Task AddAsync(StoreEntity entity, CancellationToken cancellationToken);
        Task<StoreEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
