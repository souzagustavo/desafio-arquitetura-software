using CashFlow.Application.Common;
using CashFlow.Application.Transactions.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Endpoints.Transactions
{
    public static class GetPagedEndpoint
    {
        public static IEndpointRouteBuilder MapGetPagedTransactionEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/", GetPagedAsync)
                .WithTags("Transactions")
                .WithName("GetPagedTransaction")
                .WithSummary("Get a paged list of transactions.")
                .WithDescription("Retrieves a paged list of transactions for the authenticated user.")
                .Produces<PagedResult<GetTransactionResponse>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status403Forbidden)
                .Produces(StatusCodes.Status500InternalServerError);

            return app;
        }

        private static async Task<IResult> GetPagedAsync(
            [FromRoute] Guid storeId,
            [FromQuery] GetPagedTransactionsQuery query,
            IGetPagedTransactionHandler handler,
            CancellationToken cancellationToken)
        {
            var userId = Guid.NewGuid();

            var response = await handler.HandleAsync(userId: userId, storeId: storeId, query: query, cancellationToken);

            if (response.IsError)
                return response.Errors.ToProblem();

            return Results.Ok(response.Value);
        }
    }
}
