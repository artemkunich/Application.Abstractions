using System.Text;
using Application.Symbol;

namespace Application.Space;

public sealed class SpaceCommandBehavior3 : SymbolCommandBehavior<SpaceCommand>
{
    public SpaceCommandBehavior3(StringBuilder resultBuilder) : base(resultBuilder)
    {
    }

    protected override int Order => 3;
}