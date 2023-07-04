using System.Text;
using Akunich.Application.Abstractions;
using Application.Symbol;

namespace Application.Space;

public sealed class SpaceCommandHandler : SymbolCommandHandler<SpaceCommand>
{
    private readonly INotificationDispatcher _notificationDispatcher;
    
    public SpaceCommandHandler(
        StringBuilder resultBuilder, 
        INotificationDispatcher notificationDispatcher) : base(resultBuilder)
    {
        _notificationDispatcher = notificationDispatcher;
    }

    public override Task<Result<Unit>> HandleAsync(SpaceCommand command, CancellationToken cancellation)
    {
        if (!command.RaiseNotification)
            return base.HandleAsync(command, cancellation);
        
        var notification = new SpaceNotification
        { 
            BehaviorsCount = command.BehaviorsCount,
            Value = command.Value
        };
        return _notificationDispatcher.DispatchAsync(notification, cancellation);
        
    }
}