
namespace MeuBolso.Account.Api.Endpoints
{
    public class CurrentUserEndpointFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            // Access the HttpContext to retrieve the token or claims
            var httpContext = context.HttpContext;
            var user = httpContext.User;

            if (user.Identity?.IsAuthenticated == true)
            {
                // Example: Check for a specific custom claim
                var customClaim = user.FindFirst("custom_data");
                if (customClaim == null)
                {
                    // Handle missing custom claim, e.g., return Unauthorized
                    return Results.Unauthorized();
                }
            }

            // Continue to the next filter or the endpoint handler
            return await next(context);
        }
    }
}
