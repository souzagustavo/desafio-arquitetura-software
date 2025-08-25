using MeuBolso.Api.Endpoints.Account;
using MeuBolso.Api.Endpoints.Transactions;

namespace MeuBolso.Account.Api.Endpoints;
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
