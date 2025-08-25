﻿using CashFlow.Application.Common.Handlers;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

public static class HandlerRegistrationExtensions
{
    /// <summary>
    /// Registers all handlers from the assembly containing the specified type
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="marker">A type from the assembly where handlers are located</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection RegisterHandlersFromAssemblyContaining(
    this IServiceCollection services, Type marker)
    {
        RegisterCommandHandlers(services, marker.Assembly);
        return services;
    }

    private static void RegisterCommandHandlers(IServiceCollection services, Assembly assembly)
    {
        var handlerTypes = assembly.GetTypes()
            .Where(t => t is { IsClass: true, IsAbstract: false }
                && t.IsAssignableTo(typeof(IHandler)))
            .ToList();

        foreach (var implementationType in handlerTypes)
        {
            var interfaceType = implementationType.GetInterfaces()
                .FirstOrDefault(i => i != typeof(IHandler) && i.IsAssignableTo(typeof(IHandler)));

            if (interfaceType is not null)
            {
                services.AddScoped(interfaceType, implementationType);
            }
        }
    }
}