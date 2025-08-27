using CashFlow.Domain;
using CashFlow.Infrastructure;
using CashFlow.Infrastructure.Common.PubSub;

namespace CashFlow.Consumer.Worker;

public static class HostingExtensions
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddInfrastructure(builder.Configuration);

        builder.Services.AddMassTransitWithRabbitMq(options =>
        {

        });

        builder.Services
            .AddInfrastructure(builder.Configuration)
            .AddRedLock(builder.Configuration);

        builder.Services.AddDomainServices();

        return builder;
    }
}
