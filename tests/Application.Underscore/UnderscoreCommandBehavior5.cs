using System.Text;
using Application.Symbol;

namespace Application.Underscore;

public class UnderscoreCommandBehavior5 : SymbolCommandBehavior<UnderscoreCommand>
{
    public UnderscoreCommandBehavior5(StringBuilder resultBuilder) : base(resultBuilder)
    {
    }
    protected override int Order => 5;
}