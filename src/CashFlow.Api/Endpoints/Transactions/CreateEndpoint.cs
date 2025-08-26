using CashFlow.Application.Transactions.Handlers;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Endpoints.Transactions;
public static class CreateEndpoint
{
    public static IEndpointRouteBuilder MapCreateTransactionEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/", CreateAsync)
            .WithTags("Transactions")
            .WithName("CreateTransaction")
            .WithSummary("Create a new transaction.")
            .WithDescription("Creates a new transaction for the authenticated user.")
            .ProducesValidationProblem()
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces(StatusCodes.Status403Forbidden)
            .Produces(StatusCodes.Status500InternalServerError);

        return app;
    }

    private static async Task<IResult> CreateAsync(
        [FromRoute] Guid storeId,
        [FromBody] CreateTransactionRequest request,
        IValidator<CreateTransactionRequest> validator,
        ICreateTransationHandler handler,
        CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return Results.ValidationProblem(validationResult.ToDictionary());
        }

        var response = await handler.HandleAsync(UserId: userId, StoreId: storeId, request, cancellationToken);
        if (response.IsError)
        {
            return response.Errors.ToProblem();
        }

        return Results.CreatedAtRoute("GetTransactionById", new { id = response.Value.Id }, response.Value);
    }
}
