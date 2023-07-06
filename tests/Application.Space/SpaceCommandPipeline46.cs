using Akunich.Application.Abstractions;

namespace Application.Space;

public class SpaceCommandPipeline46 : Pipeline<SpaceCommand, Unit>
{
    public SpaceCommandPipeline46(
        IRequestHandler<SpaceCommand, Unit> handler,
        SpaceCommandBehavior4 behavior1,
        SpaceCommandBehavior5 behavior2,
        SpaceCommandBehavior6 behavior3
        ) : base(handler, behavior1, behavior2, behavior3)
    {
    }
}