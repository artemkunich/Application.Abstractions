using Akunich.Application.Abstractions;

namespace Application.Symbol;

public abstract class SymbolCommand : IRequest<Unit>
{
    public abstract string Symbol { get; }
    
    public int BehaviorsCount { get; }
    public string Value { get; set; }

    public SymbolCommand(int behaviorsCount)
    {
        BehaviorsCount = behaviorsCount;
    }
}