using System.Text;
using Application.Symbol;

namespace Application.Space;

public sealed class SpaceCommandBehavior2 : SymbolCommandBehavior<SpaceCommand>
{
    public SpaceCommandBehavior2(StringBuilder resultBuilder) : base(resultBuilder)
    {
    }

    protected override int Order => 2;
}