using System.Text;
using Application.Symbol;

namespace Application.Underscore;

public class UnderscoreCommandBehavior6 : SymbolCommandBehavior<UnderscoreCommand>
{
    public UnderscoreCommandBehavior6(StringBuilder resultBuilder) : base(resultBuilder)
    {
    }
    protected override int Order => 6;
}