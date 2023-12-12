using Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FluentMediator.Internal;

internal sealed class NotificationBinder<TNotification, TRequest, TResponse> : INotificationHandler<TNotification> 
    where TNotification : INotification 
    where TRequest : IRequest<TResponse>
{
    private readonly MapNotificationDelegate<TNotification, TRequest, TResponse> _mapNotification;
    private readonly Pipeline<TRequest,TResponse> _pipeline;
    
    public NotificationBinder(
        IServiceProvider serviceProvider,
        object key = null)
    {
        _pipeline = key is null
            ? serviceProvider.GetRequiredService<Pipeline<TRequest, TResponse>>()
            : serviceProvider.GetRequiredKeyedService<Pipeline<TRequest, TResponse>>(key);
        _mapNotification = key is null 
            ? serviceProvider.GetRequiredService<MapNotificationDelegate<TNotification, TRequest, TResponse>>()
            : serviceProvider.GetRequiredKeyedService<MapNotificationDelegate<TNotification, TRequest, TResponse>>(key);

    }

    public async Task<Result> HandleAsync(TNotification notification, CancellationToken cancellation)
    {
        var command = _mapNotification(notification);
        return await _pipeline.HandleAsync(command, cancellation);
    }
}