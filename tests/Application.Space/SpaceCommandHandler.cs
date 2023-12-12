using System.Text;
using Application.Abstractions;
using Application.Symbol;

namespace Application.Space;

public sealed class SpaceCommandHandler : SymbolCommandHandler<SpaceCommand>
{
    private readonly IMediator _mediator;
    
    public SpaceCommandHandler(
        StringBuilder resultBuilder,
        IMediator mediator) : base(resultBuilder)
    {
        _mediator = mediator;
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
        var result = await _mediator.PublishAsync(notification, cancellation);
        if (result.IsFailure)
            return result.Errors;

        return Unit.Value;
    }
}