using CashFlow.Application.Store.Handlers;
using CashFlow.Domain.Store;
using Riok.Mapperly.Abstractions;

namespace CashFlow.Application.Store
{
    [Mapper]
    public partial class StoreMapper
    {
        public partial StoreEntity ToStoreEntity(CreateStoreRequest storeRequest);
        public partial GetStoreResponse ToGetStoreResponse(StoreEntity storeEntity);
    }
}
