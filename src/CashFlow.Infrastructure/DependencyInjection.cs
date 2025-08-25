using CashFlow.Application.Store;
using CashFlow.Application.Common.Storage;
using CashFlow.Application.Transactions;
using CashFlow.Infrastructure.Store;
using CashFlow.Infrastructure.Common.Persistence;
using CashFlow.Infrastructure.Common.Storage;
using CashFlow.Infrastructure.Transaction;
using Microsoft.AspNetCore.Identity;
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
        var cs = configuration.GetConnectionString("Postgres");

        services.AddDbContext<CashFlowDbContext>(options => options.UseNpgsql(cs));

        services.AddScoped<ITransationsRepository, TransactionRepository>();
        services.AddScoped<IStoreRepository, StoreRepository>();

        return services;
    }

    private static IServiceCollection AddStorage(this IServiceCollection services)
    {
        services.AddScoped<IStorageService, StorageService>();

        return services;
    }


}