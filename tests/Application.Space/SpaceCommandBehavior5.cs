using System.Text;
using Application.Symbol;

namespace Application.Space;

public sealed class SpaceCommandBehavior5 : SymbolCommandBehavior<SpaceCommand>
{
    public SpaceCommandBehavior5(StringBuilder resultBuilder) : base(resultBuilder)
    {
    }

    protected override int Order => 5;
}