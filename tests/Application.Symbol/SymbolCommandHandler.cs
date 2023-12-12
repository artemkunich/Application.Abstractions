using System.Text;
using Application.Abstractions;

namespace Application.Symbol;

public abstract class SymbolCommandHandler<TCommand> : IHandler<TCommand, Unit> where TCommand : SymbolCommand
{
    private StringBuilder _resultBuilder;
    
    public SymbolCommandHandler(StringBuilder resultBuilder)
    {
        _resultBuilder = resultBuilder;
    }

    public virtual Task<Result<Unit>> HandleAsync(TCommand command, CancellationToken cancellation)
    {
        _resultBuilder.Append(command.Symbol);
        _resultBuilder.Append(command.BehaviorsCount + 1);
        _resultBuilder.Append(command.Symbol);

        return Task.FromResult(Result.Create(Unit.Value));
    }
}