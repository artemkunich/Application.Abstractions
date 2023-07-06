using Akunich.Application.Abstractions;

namespace Application.Space;

public sealed class SpaceCommandPipeline13WithHandler : Pipeline<SpaceCommand, Unit>
{
    public SpaceCommandPipeline13WithHandler(
        IRequestHandler<SpaceCommand, Unit> handler,
        SpaceCommandBehavior1 behavior1,
        SpaceCommandBehavior2 behavior2,
        SpaceCommandBehavior3 behavior3
        
    ) : base(handler,behavior1, behavior2, behavior3)
    {
    }
}