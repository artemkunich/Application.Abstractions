using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Application.Abstractions;

namespace FluentMediator.Internal;

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
            .AddScoped<INotificationHandler<TNotification>, NotificationBinder<TNotification, TRequest, TResponse>>();

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
            .AddScoped<INotificationHandler<TNotification>, NotificationBinder<TNotification, TRequest, TResponse>>(
                sp => new NotificationBinder<TNotification, TRequest, TResponse>(sp, key)
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
        _services.AddSingleton(_typesStore);

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
       
        return this;
    }

    private MediatorConfiguration RegisterKeyedService(object key)
    {
        var typesStore = _keydTypesStoreDict[key];
        _services.AddKeyedSingleton(key, typesStore);

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

        return this;
    }

    private void RegisterKeyedPipeline<TRequest,TResponse>(object key) where TRequest : class, IRequest<TResponse>
    {
        _services.AddKeyedScoped(key, (sp, k) => new Pipeline<TRequest, TResponse>(sp, k));
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
