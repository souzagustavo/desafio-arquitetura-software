using CashFlow.Api.Endpoints.Account;
using CashFlow.Api.Endpoints.Transactions;

namespace CashFlow.Account.Api.Endpoints;
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
            .MapGroup("/account")                
                .MapCreateAccountEndpoint()
                .MapGetAccountByIdEndpoint();
    }
}
