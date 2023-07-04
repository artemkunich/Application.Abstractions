using System.Text;
using Application.Symbol;

namespace Application.Underscore;

public class UnderscoreCommandBehavior4 : SymbolCommandBehavior<UnderscoreCommand>
{
    public UnderscoreCommandBehavior4(StringBuilder resultBuilder) : base(resultBuilder)
    {
    }
    protected override int Order => 4;
}