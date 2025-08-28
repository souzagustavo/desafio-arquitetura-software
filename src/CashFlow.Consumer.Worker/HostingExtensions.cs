using CashFlow.Consumer.Worker.Consumers;
using CashFlow.Infrastructure;
using CashFlow.Infrastructure.Common.PubSub;
using MassTransit;

namespace CashFlow.Consumer.Worker;

public static class HostingExtensions
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddPersistence(builder.Configuration)
            .AddRedLock(builder.Configuration)
            .AddCache(builder.Configuration);

        builder.Services.AddMassTransitDefaults(configure =>
        {
            configure.AddConsumers(typeof(ProcessTransactionOnCreated).Assembly);

            configure.UsingRabbitMq((context, cfg) =>
            {
                cfg.AddRabbitMqHost(context);
                cfg.AddProcessTransactionCreatedConsumer(context);
            });
        });

        return builder;
    }
}
