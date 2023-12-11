using System;

namespace Akunich.Application.Abstractions;

public interface IMediatorConfiguration
{
    IMediatorConfiguration ConfigurePipeline<TRequest, TResponse>(
        Func<IPipelineConfiguration<TRequest, TResponse>, IPipelineConfigurationCompleted<TRequest, TResponse>> buildFunc)
        where TRequest : IRequest<TResponse>;

    IMediatorConfiguration ConfigurePipeline<TRequest, TResponse>(
        object key,
        Func<IPipelineConfiguration<TRequest, TResponse>, IPipelineConfigurationCompleted<TRequest, TResponse>> buildFunc)
        where TRequest : IRequest<TResponse>;

    IMediatorConfiguration BindNotification<TNotification, TRequest, TResponse>(
        MapNotificationDelegate<TNotification, TRequest, TResponse> mapNotification)
        where TNotification : INotification
        where TRequest : IRequest<TResponse>;

    IMediatorConfiguration BindNotification<TNotification, TRequest, TResponse>(
        object key,
        MapNotificationDelegate<TNotification, TRequest, TResponse> mapNotification)
        where TNotification : INotification
        where TRequest : IRequest<TResponse>;
}
