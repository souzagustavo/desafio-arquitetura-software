using FluentValidation;
using CashFlow.Application.Transactions.Handlers;
using CashFlow.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using CashFlow.Application.Sales.Handlers;

namespace CashFlow.Api;

public static class HostingExtensions
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .ConfigureEndpoints()
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();        

        builder.Services
            .AddInfrastructure(builder.Configuration);

        builder.Services
            .AddValidatorsFromAssemblyContaining<CreatePurchaseValidator>()
            .RegisterHandlersFromAssemblyContaining(typeof(CreatePurchaseHandler));

        return builder;
    }

    private static IServiceCollection ConfigureEndpoints(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "CashFlow Api",
                Description = "Uma Web API que gerencia transações financeiras das suas lojas",
                Contact = new OpenApiContact
                {
                    Name = "Gustavo Souza Silva",
                    Url = new Uri("https://www.linkedin.com/in/gustavo-souza-silva")
                }
            });
            options.SchemaFilter<EnumSchemaFilter>();
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Insira o token JWT desta maneira: Bearer {seu token}"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        return services;
    }
}
