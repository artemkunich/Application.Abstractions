using System;
using FluentMediator.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace FluentMediator;

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