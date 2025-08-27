using CashFlow.Domain.Account;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Domain
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountBalanceService, AccountBalanceService>();

            return services;
        }
    }
}
