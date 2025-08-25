using CashFlow.Application.Store.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Endpoints.Store
{
    public static class CreateEndpoint
    {
        public static RouteGroupBuilder MapCreateStoreEndpoint(this RouteGroupBuilder group)
        {
            group.MapPost("/", CreateAsync)
                .WithTags("Stores")
                .WithName("CreateStore")
                .WithSummary("Create a new store.")
                .WithDescription("Creates a new store for the authenticated user.")
                .ProducesValidationProblem()
                .Produces(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status403Forbidden)
                .Produces(StatusCodes.Status409Conflict);
            
            return group;
        }

        private static async Task<IResult> CreateAsync(
            [FromBody] CreateStoreRequest request,
            ICreateStoreHandler handler,
            CancellationToken cancellationToken)
        {
            var response = await handler.HandleAsync(request, cancellationToken);
            if (response.IsError)
            {
                return response.Errors.ToProblem();
            }

            return Results.CreatedAtRoute("GetStoreById", new { id = response.Value.Id }, response.Value);
        }
    }
}
