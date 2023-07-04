using System.Text;
using Akunich.Application.Abstractions;

namespace Application.Symbol;

public abstract class SymbolCommandBehavior<TCommand> : IPipelineBehavior<TCommand, Unit> where TCommand : SymbolCommand
{
    private readonly StringBuilder _resultBuilder;
    
    public SymbolCommandBehavior(StringBuilder resultBuilder)
    {
        _resultBuilder = resultBuilder;
    }

    public async Task<Result<Unit>> HandleAsync(TCommand command, CancellationToken cancellation, NextDelegate<Unit> nextAsync)
    {
        _resultBuilder.Append(command.Symbol + GetBehaviorBefore());
        var result = await nextAsync();
        _resultBuilder.Append(GetBehaviorAfter(command.BehaviorsCount) + command.Symbol);
        return result;
    }

    protected abstract int Order { get; }
    
    private int GetBehaviorBefore() => Order;
    
    private int GetBehaviorAfter(int behaviorsCount) => 2 * behaviorsCount + 2 - Order;
}