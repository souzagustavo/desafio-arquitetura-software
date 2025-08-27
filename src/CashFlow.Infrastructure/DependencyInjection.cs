using CashFlow.Application.Common.Interfaces;
using CashFlow.Application.Common.Storage;
using CashFlow.Application.Transactions;
using CashFlow.Infrastructure.Common.Cache;
using CashFlow.Infrastructure.Common.Persistence;
using CashFlow.Infrastructure.Common.Storage;
using CashFlow.Infrastructure.Transaction;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;

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

    public static IServiceCollection AddRedLock(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RedLockRootOptions>(configuration.GetSection(RedLockRootOptions.SectionName));

        services.AddSingleton<IDistributedLockFactory>(sp =>
        {
            var cs = configuration.GetConnectionString("Redis");

            var multiplexer = new List<RedLockMultiplexer> {
            ConnectionMultiplexer.Connect(cs)
            };

            return RedLockFactory.Create(multiplexer);
        });

        services.AddScoped<ITransactionLockProcessor, TransactionLockProcessor>();

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