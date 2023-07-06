using Akunich.Application.Abstractions;

namespace Application.Underscore;

public sealed class UnderscoreCommandPipeline46 : Pipeline<UnderscoreCommand, Unit>
{
    public UnderscoreCommandPipeline46(
        IRequestHandler<UnderscoreCommand, Unit> handler,
        UnderscoreCommandBehavior4 behavior4,
        UnderscoreCommandBehavior5 behavior5,
        UnderscoreCommandBehavior6 behavior6
    ) : base(handler, behavior4, behavior5, behavior6)
    {
        
    }
}