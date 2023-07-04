using System.Text;
using Application.Symbol;

namespace Application.Underscore;

public class UnderscoreCommandHandler : SymbolCommandHandler<UnderscoreCommand>
{
    public UnderscoreCommandHandler(StringBuilder resultBuilder) : base(resultBuilder)
    {
    }
}