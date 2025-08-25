using MeuBolso.Application.Transactions.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace MeuBolso.Api.Endpoints.Transactions;

public static class GetByIdEndpoint
{
    public static IEndpointRouteBuilder MapGetTransactionByIdEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/{id:guid}", GetByIdAsync)
            .WithTags("Transactions")
            .WithName("GetTransactionById")
            .WithSummary("Get a transaction by ID.")
            .WithDescription("Retrieves a transaction by its ID for the authenticated user.")
            .Produces<GetTransactionResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

        return app;
    }

    private static async Task<IResult> GetByIdAsync(
        [FromRoute] Guid id,
        IGetTransactionByIdHandler handler,
        CancellationToken cancellationToken)
    {
        var response = await handler.HandleAsync(id, cancellationToken);
        if (response.IsError)
        {
            return response.Errors.ToProblem();
        }

        return Results.Ok(response.Value);
    }
}
