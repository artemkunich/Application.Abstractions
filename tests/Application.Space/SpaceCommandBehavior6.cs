using System.Text;
using Application.Symbol;

namespace Application.Space;

public sealed class SpaceCommandBehavior6 : SymbolCommandBehavior<SpaceCommand>
{
    public SpaceCommandBehavior6(StringBuilder resultBuilder) : base(resultBuilder)
    {
    }

    protected override int Order => 6;
}