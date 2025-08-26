using CashFlow.Application.Common.Handlers;
using CashFlow.Application.Common.Interfaces;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Application.Store.Handlers;

public record GetStoreResponse(Guid Id, string Name);

public interface IGetStoreByIdHandler : IHandler
{
    Task<ErrorOr<GetStoreResponse>> HandleAsync(
        Guid userId,
        Guid id,
        CancellationToken cancellationToken);
}

public class GetStoreByIdHandler : IGetStoreByIdHandler
{
    private readonly ICashFlowDbContext _cashFlowDbContext;

    public GetStoreByIdHandler(ICashFlowDbContext cashFlowDbContext)
    {
        _cashFlowDbContext = cashFlowDbContext;
    }

    public async Task<ErrorOr<GetStoreResponse>> HandleAsync(Guid userId, Guid id, CancellationToken cancellationToken)
    {
        var store =
            await _cashFlowDbContext.Stores
                .FirstOrDefaultAsync(s => s.IdentityUserId == userId && s.Id == id, cancellationToken);

        if (store is null)
            return Error.NotFound(description: "Store not found.");

        var mapper = new StoreMapper();
        var response = mapper.ToGetStoreResponse(store);

        return response;
    }
}
