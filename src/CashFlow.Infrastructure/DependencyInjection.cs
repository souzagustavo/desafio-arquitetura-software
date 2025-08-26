using CashFlow.Application.Common.Interfaces;
using CashFlow.Application.Common.Storage;
using CashFlow.Infrastructure.Common.Persistence;
using CashFlow.Infrastructure.Common.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddPersistence(configuration);
        //.AddStorage();

        return services;
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var cs = configuration.GetConnectionString("CashFlowDb");

        services.AddDbContext<ICashFlowDbContext, CashFlowDbContext>(options => options.UseNpgsql(cs));

        return services;
    }

    private static IServiceCollection AddStorage(this IServiceCollection services)
    {
        services.AddScoped<IStorageService, StorageService>();

        return services;
    }


}