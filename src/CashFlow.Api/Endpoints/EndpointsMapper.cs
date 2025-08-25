using CashFlow.Api.Endpoints.Store;
using CashFlow.Api.Endpoints.Transactions;

namespace CashFlow.Store.Api.Endpoints;
public static class EndpointGroupMapper
{
    public static void MapAllEndpoints(this WebApplication app)
    {
        app.MapGroup("/me")
            .RequireAuthorization()
            //.AddEndpointFilter<CurrentUserEndpointFilter>()
            .MapGroup("/transactions")
                .MapCreateTransactionEndpoint()
                .MapGetTransactionByIdEndpoint()                
            .MapGroup("/store")                
                .MapCreateStoreEndpoint()
                .MapGetStoreByIdEndpoint();
    }
}
