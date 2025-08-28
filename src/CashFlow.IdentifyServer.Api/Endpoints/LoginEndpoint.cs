using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CashFlow.IdentifyServer.Api.Endpoints;

public static class LoginEndpoint
{
    public record LoginRequest(string Email, string Password);

    public static WebApplication MapLoginEndpoint(this WebApplication app)
    {
        app.MapPost("/login", async (LoginRequest req, UserManager<IdentityUser<Guid>> userManager) =>
        {
            var user = await userManager.FindByEmailAsync(req.Email);
            if (user is null || !await userManager.CheckPasswordAsync(user, req.Password))
                return Results.Unauthorized();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
            };

            // for testing
            var key = new SymmetricSecurityKey(HostingExtensions.IssuerSecurityKey);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                //issuer: HostingExtensions.ValidIssuer,
                //audience: HostingExtensions.ValidAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(6),
                signingCredentials: creds
            );

            return Results.Ok(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(token),
                expiresIn = 3600
            });
        });

        return app;
    }
}
