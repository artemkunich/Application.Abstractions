using System;
using Akunich.Application.Abstractions.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Akunich.Application.Abstractions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services, Func<IMediatorConfiguration,IMediatorConfiguration> configurationAction)
    {
        var configuration = new MediatorConfiguration(services);
        configurationAction(configuration);
        configuration.RegisterServices();

        return services;
    }
}