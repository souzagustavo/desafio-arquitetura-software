using CashFlow.Application.Account;
using CashFlow.Application.Common.Storage;
using CashFlow.Application.Transactions;
using CashFlow.Domain.User;
using CashFlow.Infrastructure.Account;
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

    public static IdentityBuilder AddIdentityServer(this IServiceCollection services)
    {
        return services.AddIdentityCore<UserEntity>()
            .AddEntityFrameworkStores<CashFlowDbContext>();        
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var cs = configuration.GetConnectionString("Postgres");

        services.AddDbContext<CashFlowDbContext>(options => options.UseNpgsql(cs));

        services.AddScoped<ITransationsRepository, TransactionRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();

        return services;
    }

    private static IServiceCollection AddStorage(this IServiceCollection services)
    {
        services.AddScoped<IStorageService, StorageService>();

        return services;
    }

    
}