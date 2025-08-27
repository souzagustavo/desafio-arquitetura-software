using CashFlow.Application.Common.PubSub;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CashFlow.Infrastructure.Common.PubSub
{
    public static class MassTransitExtensions
    {
        public static IServiceCollection AddMassTransitWithRabbitMq(this IServiceCollection services,
            Action<IBusRegistrationConfigurator> configure = null)
        {
            services.AddScoped<IBusPublisher, BusPublisher>();

            services.ConfigureOptions<ConfigureRabbitMqOptions>();

            return services.AddMassTransit((x) =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.UseRabbitMqHost(context);
                    cfg.UseRawJsonSerializer();
                    cfg.ConfigureJsonSerializerOptions(jsonSerializerSettings =>
                    {
                        jsonSerializerSettings.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
                        return jsonSerializerSettings;
                    });
                });

                if (configure is not null)
                    configure(x);
            });            
        }

        public static void UseRabbitMqHost(this IRabbitMqBusFactoryConfigurator configurator, IBusRegistrationContext context)
        {
            var rabbitMqOptions = context.GetRequiredService<IOptions<RabbitMqOptions>>().Value;
            configurator.Host(rabbitMqOptions.ConnectionString);
        }
    }
}
