using CashFlow.IdentifyServer.Api.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

internal class Program
{
    private static async Task Main(string[] args)
    {
        // Configuração do builder
        var builder = WebApplication.CreateBuilder(args);

        // DbContext Identity
        var cs = builder.Configuration.GetConnectionString("IdentityServerDb");
        builder.Services.AddDbContext<IdentityServerDbContext>(opt =>
            opt.UseNpgsql(cs));

        // Identity Core
        builder.Services.AddIdentityCore<IdentityUser<Guid>>(opt =>
        {
            opt.User.RequireUniqueEmail = true;
            opt.Password.RequireNonAlphanumeric = false;
        })
        .AddEntityFrameworkStores<IdentityServerDbContext>();

        // JWT Authentication
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = "JwtBearer";
            options.DefaultChallengeScheme = "JwtBearer";
        })
        .AddJwtBearer("JwtBearer", options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
        });

        builder.Services.AddAuthorization();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<IdentityServerDbContext>();
            await context.Database.MigrateAsync();
        }

        // -------------------------
        // ENDPOINTS
        // -------------------------

        app.MapPost("/register", async (RegisterRequest req, UserManager<IdentityUser<Guid>> userManager) =>
        {
            var user = new IdentityUser<Guid> { UserName = req.Email, Email = req.Email };
            var result = await userManager.CreateAsync(user, req.Password);

            if (!result.Succeeded)
                return Results.BadRequest(result.Errors);

            return Results.Ok(new { user.Id, user.Email });
        });

        app.MapPost("/login", async (LoginRequest req, UserManager<IdentityUser<Guid>> userManager) =>
        {
            var user = await userManager.FindByEmailAsync(req.Email);
            if (user is null || !await userManager.CheckPasswordAsync(user, req.Password))
                return Results.Unauthorized();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: builder.Configuration["Jwt:Issuer"],
                audience: builder.Configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return Results.Ok(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(token),
                expiresIn = 3600
            });
        });

        app.MapPost("/reset-password", async (ResetPasswordRequest req, UserManager<IdentityUser<Guid>> userManager) =>
        {
            var user = await userManager.FindByEmailAsync(req.Email);
            if (user is null)
                return Results.NotFound();

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var resetResult = await userManager.ResetPasswordAsync(user, token, req.NewPassword);

            if (!resetResult.Succeeded)
                return Results.BadRequest(resetResult.Errors);

            return Results.Ok();
        });

        app.Run();
    }
}

// -------------------------
// RECORDS
// -------------------------
public record RegisterRequest(string Email, string Password);
public record LoginRequest(string Email, string Password);
public record ResetPasswordRequest(string Email, string NewPassword);
