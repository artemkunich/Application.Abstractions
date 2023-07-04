using System.Text;
using Application.Symbol;

namespace Application.Space;

public sealed class SpaceCommandBehavior1 : SymbolCommandBehavior<SpaceCommand>
{
    public SpaceCommandBehavior1(StringBuilder resultBuilder) : base(resultBuilder)
    {
    }

    protected override int Order => 1;
}