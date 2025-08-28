using CashFlow.IdentifyServer.Api.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CashFlow.IdentifyServer.Api;

public static class HostingExtensions
{
    public static byte[] IssuerSecurityKey { get => Encoding.UTF8.GetBytes("Wn8dKp3rL9mQz7tVx2uB5cY4eG1hJ6kP"); }
    public static string ValidIssuer = "cashflow-identity-server";
    public static string ValidAudience = "cashflow-api";

    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddDatabase(builder.Configuration);

        builder.Services
            .AddIdentityCore<IdentityUser<Guid>>(options =>
            {
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<IdentityServerDbContext>();

        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateLifetime = true,
                
                ValidateIssuerSigningKey = true,                
                IssuerSigningKey = new SymmetricSecurityKey(IssuerSecurityKey)
            };
        });

        return builder;
    }
}