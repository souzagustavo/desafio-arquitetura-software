using CashFlow.Api.Endpoints;
using CashFlow.Api.Endpoints.Account;
using CashFlow.Api.Endpoints.Accounts;
using CashFlow.Api.Endpoints.Transactions;

namespace CashFlow.Account.Api.Endpoints;
public static class EndpointGroupMapper
{
    public static void MapAllEndpoints(this WebApplication app)
    {
        app.MapGroup("/me")
            .RequireAuthorization()
            .AddEndpointFilter<RequireUserIdFilter>()
            .ProducesValidationProblem()
            .MapGroup("/accounts")
                .MapCreateAccountEndpoint()
                .MapGetAccountByIdEndpoint()
                .MapGetPagedAccountsEndpoint()
            .MapGroup("/{accountId:guid}/transactions")
                .MapCreateTransactionEndpoint()
                .MapGetTransactionByIdEndpoint()
                .MapGetPagedTransactionsEndpoint()
                
                ;
    }
}
