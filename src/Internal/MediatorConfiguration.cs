using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Akunich.Application.Abstractions.Internal;

internal class MediatorConfiguration : IMediatorConfiguration
{
    private readonly MediatorTypesStore _typesStore;
    private readonly Dictionary<object, MediatorTypesStore> _keydTypesStoreDict;
    private readonly IServiceCollection _services;


    public MediatorConfiguration(IServiceCollection services)
    {
        _typesStore = new MediatorTypesStore();
        _keydTypesStoreDict = new Dictionary<object, MediatorTypesStore>();
        _services = services;
    }

    public IMediatorConfiguration ConfigurePipeline<TRequest, TResponse>(
        Func<IPipelineConfiguration<TRequest,TResponse>,IPipelineConfigurationCompleted<TRequest,TResponse>> buildFunc)
        where TRequest : IRequest<TResponse>
    {
        var typesStore = _typesStore;
        var pipelineBuilder = new PipelineConfiguration<TRequest,TResponse>(typesStore);
        buildFunc(pipelineBuilder);
        return this;
    }

    public IMediatorConfiguration ConfigurePipeline<TRequest, TResponse>(
        object key,
        Func<IPipelineConfiguration<TRequest, TResponse>, IPipelineConfigurationCompleted<TRequest, TResponse>> buildFunc)
        where TRequest : IRequest<TResponse>
    {
        if(!_keydTypesStoreDict.ContainsKey(key))
            _keydTypesStoreDict[key] = new MediatorTypesStore();
        var typesStore = _keydTypesStoreDict[key];
        var pipelineBuilder = new PipelineConfiguration<TRequest, TResponse>(typesStore);
        buildFunc(pipelineBuilder);
        return this;
    }

    public IMediatorConfiguration BindNotification<TNotification, TRequest, TResponse>(
        MapNotificationDelegate<TNotification, TRequest, TResponse> mapNotification)
        where TNotification : INotification
        where TRequest : IRequest<TResponse>
    {
        _services
            .AddSingleton(mapNotification)
            .AddScoped<INotificationHandler<TNotification>,NotificationMediator<TNotification, TRequest, TResponse>>();

        return this;
    }

    public IMediatorConfiguration BindNotification<TNotification, TRequest, TResponse>(
        object key,
        MapNotificationDelegate<TNotification, TRequest, TResponse> mapNotification)
        where TNotification : INotification
        where TRequest : IRequest<TResponse>
    {
        _services
            .AddKeyedSingleton(key, mapNotification)
            .AddScoped<INotificationHandler<TNotification>, NotificationMediator<TNotification, TRequest, TResponse>>(
                sp => new NotificationMediator<TNotification, TRequest, TResponse>(
                    sp.GetRequiredKeyedService<MapNotificationDelegate<TNotification,TRequest,TResponse>>(key)
                    ,sp.GetRequiredKeyedService<Pipeline<TRequest,TResponse>>(key)                
                )
             );

        return this;
    }


    public MediatorConfiguration RegisterServices()
    {
        RegisterDefaultServices();

        foreach(var keyValue in _keydTypesStoreDict)
        {
            RegisterKeyedService(keyValue.Key);
        }

        _services.AddScoped<IMediator, Mediator>();
        return this;
    }

    private MediatorConfiguration RegisterDefaultServices()
    {
        var behaviors = _typesStore.GetBehaviors();
        foreach(var type in behaviors)
        {
            _services.AddScoped(type);
        }

        var handlers = _typesStore.GetHandlers();
        foreach (var keyValue in handlers)
        {
            _services.AddScoped(keyValue.Key, keyValue.Value);

            var requestType = _typesStore.HandlerRequests[keyValue.Key];
            var responseType = _typesStore.HandlerResponses[keyValue.Key];
            var pipelineType = typeof(Pipeline<,>).MakeGenericType(requestType, responseType);
            _services.AddScoped(pipelineType);
        }

        _services.AddSingleton(_typesStore);
        return this;
    }

    private MediatorConfiguration RegisterKeyedService(object key)
    {
        var typesStore = _keydTypesStoreDict[key];
        var behaviors = typesStore.GetBehaviors();
        foreach (var type in behaviors)
        {
            _services.AddKeyedScoped(type,key);
        }

        var handlers = typesStore.GetHandlers();
        foreach (var keyValue in handlers)
        {
            _services.AddKeyedScoped(keyValue.Key, key, keyValue.Value);

            var requestType = typesStore.HandlerRequests[keyValue.Key];
            var responseType = typesStore.HandlerResponses[keyValue.Key];
            var registerPipelineMethod = GetType()
                .GetMethod(nameof(RegisterKeyedPipeline), BindingFlags.NonPublic | BindingFlags.Instance)
                .MakeGenericMethod(requestType, responseType);

            registerPipelineMethod.Invoke(this, new []{ key });          
        }

        _services.AddKeyedSingleton(typesStore, key);
        return this;
    }

    private void RegisterKeyedPipeline<TRequest,TResponse>(object key) where TRequest : class, IRequest<TResponse>
    {
        _services.AddKeyedScoped(key, (sp, k) => new Pipeline<TRequest, TResponse>(
            sp.GetRequiredKeyedService<IRequestHandler<TRequest, TResponse>>(k),
            _keydTypesStoreDict[k], sp, k));
    }

    //public MediatorConfiguration AddHandler<TRequest, TResponse, THandler>()
    //    where TRequest : IRequest<TResponse>
    //    where THandler : class, IRequestHandler<TRequest, TResponse>
    //{
    //    var pipelineBuilder = new PipelineConfiguration<TRequest, TResponse>(_typesStore);
    //    pipelineBuilder.SetHandler<THandler>();
    //    return this;
    //}
}
