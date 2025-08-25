using MeuBolso.Application.Account;
using MeuBolso.Application.Common.Storage;
using MeuBolso.Application.Transactions;
using MeuBolso.Domain.User;
using MeuBolso.Infrastructure.Account;
using MeuBolso.Infrastructure.Common.Persistence;
using MeuBolso.Infrastructure.Common.Storage;
using MeuBolso.Infrastructure.Transaction;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeuBolso.Infrastructure;

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
            .AddEntityFrameworkStores<MeuBolsoDbContext>();        
    }

    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var cs = configuration.GetConnectionString("Postgres");

        services.AddDbContext<MeuBolsoDbContext>(options => options.UseNpgsql(cs));

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