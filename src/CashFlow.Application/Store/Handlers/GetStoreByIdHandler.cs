using CashFlow.Application.Common.Handlers;
using ErrorOr;

namespace CashFlow.Application.Store.Handlers;

public record GetStoreResponse(Guid Id, string Name);

public interface IGetStoreByIdHandler : IHandler
{
    Task<ErrorOr<GetStoreResponse>> HandleAsync(
        Guid Id,
        CancellationToken cancellationToken);
}

public class GetStoreByIdHandler : IGetStoreByIdHandler
{
    private readonly IStoreRepository _storeRepository;
    public GetStoreByIdHandler(IStoreRepository storeRepository)
    {
        _storeRepository = storeRepository;
    }

    public async Task<ErrorOr<GetStoreResponse>> HandleAsync(Guid id, CancellationToken cancellationToken)
    {
        var store = await _storeRepository.GetByIdAsync(id, cancellationToken);

        if (store is null)
            return Error.NotFound(description: "Store not found.");

        var mapper = new StoreMapper();
        var response = mapper.ToGetStoreResponse(store);

        return response;
    }
}
