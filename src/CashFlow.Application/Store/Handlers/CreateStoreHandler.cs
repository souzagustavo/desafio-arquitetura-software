using CashFlow.Application.Common.Handlers;
using ErrorOr;

namespace CashFlow.Application.Store.Handlers
{
    public record CreateStoreRequest(Guid UserId, string Name);

    public record CreateStoreResponse(Guid Id);

    public interface ICreateStoreHandler : IHandler
    {
        Task<ErrorOr<CreateStoreResponse>> HandleAsync(
            CreateStoreRequest request,
            CancellationToken cancellationToken);
    }

    public class CreateStoreHandler : ICreateStoreHandler
    {
        private readonly IStoreRepository _storeRepository;

        public CreateStoreHandler(IStoreRepository storeRepository)
        {
            _storeRepository = storeRepository;
        }

        public async Task<ErrorOr<CreateStoreResponse>> HandleAsync(CreateStoreRequest request, CancellationToken cancellationToken)
        {
            var mapper = new StoreMapper();
            var entity = mapper.ToStoreEntity(request);

            await _storeRepository.AddAsync(entity, cancellationToken);

            return new CreateStoreResponse(entity.Id);
        }
    }
}
