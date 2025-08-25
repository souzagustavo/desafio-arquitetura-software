using CashFlow.Application.Store.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Endpoints.Store
{
    public static class GetByIdEndpoint
    {
        public static RouteGroupBuilder MapGetStoreByIdEndpoint(this RouteGroupBuilder group)
        {
            group.MapGet("/{id:guid}", GetByIdAsync)
                .WithTags("Stores")
                .WithName("GetStoreById")
                .WithSummary("Get an store by ID.")
                .WithDescription("Retrieves an store by its ID for the authenticated user.")
                .Produces<GetStoreResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status403Forbidden)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
        private static async Task<IResult> GetByIdAsync(
            [FromRoute] Guid id,
            IGetStoreByIdHandler handler,
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
}
