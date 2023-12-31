using System.Threading;
using System.Threading.Tasks;

namespace Akunich.Application.Abstractions.Internal;

internal sealed class NotificationMediator<TNotification, TRequest, TResponse> : INotificationHandler<TNotification> 
    where TNotification : INotification 
    where TRequest : IRequest<TResponse>
{
    private readonly MapNotificationDelegate<TNotification, TRequest, TResponse> _mapNotification;
    private readonly IRequestHandler<TRequest, TResponse> _commandHandler;
    
    public NotificationMediator(MapNotificationDelegate<TNotification, TRequest, TResponse> mapNotification, IRequestHandler<TRequest, TResponse> commandHandler)
    {
        _commandHandler = commandHandler;
        _mapNotification = mapNotification;

    }

    public async Task<Result> HandleAsync(TNotification notification, CancellationToken cancellation)
    {
        var command = _mapNotification(notification);
        return await _commandHandler.HandleAsync(command, cancellation);
    }
}

internal sealed class NotificationMediator<TNotification, TRequest, TResponse, TPipeline> : INotificationHandler<TNotification> 
    where TNotification : INotification 
    where TRequest : IRequest<TResponse>
    where TPipeline : IPipeline<TRequest, TResponse>
{
    private readonly MapNotificationDelegate<TNotification, TRequest, TResponse> _mapNotification;
    private readonly TPipeline _pipeline;
    
    public NotificationMediator(MapNotificationDelegate<TNotification, TRequest, TResponse> mapNotification, TPipeline pipeline)
    {
        _mapNotification = mapNotification;
        _pipeline = pipeline;
    }

    public async Task<Result> HandleAsync(TNotification notification, CancellationToken cancellation)
    {
        var command = _mapNotification(notification);
        return await _pipeline.HandleAsync(command, cancellation);
    }
}