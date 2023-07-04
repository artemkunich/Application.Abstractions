using Application.Symbol;

namespace Application.Underscore;

public class UnderscoreCommand : SymbolCommand
{
    public override string Symbol => "_";

    public UnderscoreCommand(int behaviorsCount) : base(behaviorsCount)
    {
    }
}