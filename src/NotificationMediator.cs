using System.Threading;
using System.Threading.Tasks;

namespace Akunich.Application.Abstractions;

public abstract class NotificationMediator<TNotification, TRequest> : INotificationHandler<TNotification> 
    where TNotification : INotification 
    where TRequest : IRequest<Unit>
{
    private readonly IRequestHandler<TRequest, Unit> _commandHandler;
    
    public NotificationMediator(IRequestHandler<TRequest, Unit> commandHandler)
    {
        _commandHandler = commandHandler;
    }

    public async Task<Result<Unit>> HandleAsync(TNotification notification, CancellationToken cancellation)
    {
        var command = Map(notification);
        return await _commandHandler.HandleAsync(command, cancellation);
    }
    
    protected abstract TRequest Map(TNotification notification);
}

public abstract class NotificationMediator<TNotification, TRequest, TPipeline> : INotificationHandler<TNotification> 
    where TNotification : INotification 
    where TRequest : IRequest<Unit>
    where TPipeline : IPipeline<TRequest, Unit>
{
    private readonly TPipeline _pipeline;
    
    public NotificationMediator(TPipeline pipeline)
    {
        _pipeline = pipeline;
    }

    public async Task<Result<Unit>> HandleAsync(TNotification notification, CancellationToken cancellation)
    {
        var command = Map(notification);
        return await _pipeline.HandleAsync(command, cancellation);
    }
    
    protected abstract TRequest Map(TNotification notification);
}