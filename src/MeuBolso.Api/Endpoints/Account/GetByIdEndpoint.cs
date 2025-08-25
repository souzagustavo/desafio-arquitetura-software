using MeuBolso.Application.Account.Handlers;
using Microsoft.AspNetCore.Mvc;

namespace MeuBolso.Api.Endpoints.Account
{
    public static class GetByIdEndpoint
    {
        public static RouteGroupBuilder MapGetAccountByIdEndpoint(this RouteGroupBuilder group)
        {
            group.MapGet("/{id:guid}", GetByIdAsync)
                .WithTags("Accounts")
                .WithName("GetAccountById")
                .WithSummary("Get an account by ID.")
                .WithDescription("Retrieves an account by its ID for the authenticated user.")
                .Produces<GetAccountResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .Produces(StatusCodes.Status403Forbidden)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
        private static async Task<IResult> GetByIdAsync(
            [FromRoute] Guid id,
            IGetAccountByIdHandler handler,
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
