using System.Text;
using Application.Symbol;

namespace Application.Space;

public sealed class SpaceCommandBehavior4 : SymbolCommandBehavior<SpaceCommand>
{
    public SpaceCommandBehavior4(StringBuilder resultBuilder) : base(resultBuilder)
    {
    }

    protected override int Order => 4;
}