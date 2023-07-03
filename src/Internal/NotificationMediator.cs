using System.Threading;
using System.Threading.Tasks;

namespace Akunich.Application.Abstractions.Internal;

internal sealed class NotificationMediator<TNotification, TRequest> : INotificationHandler<TNotification> 
    where TNotification : INotification 
    where TRequest : IRequest<Unit>
{
    private readonly MapNotificationDelegate<TNotification, TRequest> _mapNotification;
    private readonly IRequestHandler<TRequest, Unit> _commandHandler;
    
    public NotificationMediator(MapNotificationDelegate<TNotification, TRequest> mapNotification, IRequestHandler<TRequest, Unit> commandHandler)
    {
        _commandHandler = commandHandler;
        _mapNotification = mapNotification;

    }

    public async Task<Result<Unit>> HandleAsync(TNotification notification, CancellationToken cancellation)
    {
        var command = _mapNotification(notification);
        return await _commandHandler.HandleAsync(command, cancellation);
    }
}

internal sealed class NotificationMediator<TNotification, TRequest, TPipeline> : INotificationHandler<TNotification> 
    where TNotification : INotification 
    where TRequest : IRequest<Unit>
    where TPipeline : IPipeline<TRequest, Unit>
{
    private readonly MapNotificationDelegate<TNotification, TRequest> _mapNotification;
    private readonly TPipeline _pipeline;
    
    public NotificationMediator(MapNotificationDelegate<TNotification, TRequest> mapNotification, TPipeline pipeline)
    {
        _mapNotification = mapNotification;
        _pipeline = pipeline;
    }

    public async Task<Result<Unit>> HandleAsync(TNotification notification, CancellationToken cancellation)
    {
        var command = _mapNotification(notification);
        return await _pipeline.HandleAsync(command, cancellation);
    }
}