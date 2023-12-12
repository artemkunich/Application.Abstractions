using Application.Abstractions;
using FluentMediator.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FluentMediator;

public class MediatorFactory
{
    private readonly IServiceCollection _services;

    public MediatorFactory(Action<IMediatorConfiguration> configurationAction)
    {
        _services = new ServiceCollection();
        var configuration = new MediatorConfiguration(_services);
        configurationAction(configuration);
        configuration.RegisterServices();
    }

    public IMediator Create()
    {
        var serviceProvider = _services.BuildServiceProvider();
        return serviceProvider.GetService<IMediator>();
    }
}
