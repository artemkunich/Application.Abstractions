using Akunich.Application.Abstractions.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Akunich.Application.Abstractions;

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
