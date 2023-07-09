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

    public override async Task<Result<Unit>> HandleAsync(SpaceCommand command, CancellationToken cancellation)
    {
        if (!command.RaiseNotification)
            return await base.HandleAsync(command, cancellation);
        
        var notification = new SpaceNotification
        { 
            BehaviorsCount = command.BehaviorsCount,
            Value = command.Value
        };
        var result = await _notificationDispatcher.DispatchAsync(notification, cancellation);
        if (result.IsFailure)
            return result.Errors;

        return Unit.Value;
    }
}