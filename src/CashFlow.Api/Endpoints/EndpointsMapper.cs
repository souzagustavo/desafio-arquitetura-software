using CashFlow.Api.Endpoints;
using CashFlow.Api.Endpoints.Accounts;
using CashFlow.Api.Endpoints.Accounts.Balance;
using CashFlow.Api.Endpoints.Accounts.Transactions;

namespace CashFlow.Account.Api.Endpoints;
public static class EndpointGroupMapper
{
    public static void MapAllEndpoints(this WebApplication app)
    {
        var meRoutes =
            app.MapGroup("/me")
                .RequireAuthorization()
                .AddEndpointFilter<RequireUserIdFilter>()
                .ProducesValidationProblem();

        var accounts =
            meRoutes.MapGroup("/accounts")
                .MapCreateAccountEndpoint()
                .MapGetAccountByIdEndpoint()
                .MapGetPagedAccountsEndpoint();

        var balance =
            accounts.MapGroup("/{accountId:guid}/balance")
                .MapGetBalanceByAccountIdEndpoint();

        var transactions =
            accounts.MapGroup("/{accountId:guid}/transactions")
                .MapCreateTransactionEndpoint()
                .MapGetTransactionByIdEndpoint()
                .MapGetPagedTransactionsEndpoint()
            ;
    }
}
