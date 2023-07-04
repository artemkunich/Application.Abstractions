using Akunich.Application.Abstractions;

namespace Application.Space;

public class SpaceCommandPipeline46 : IPipeline<SpaceCommand, Unit>
{
    private IPipeline<SpaceCommand, Unit> _pipeline;
    
    public SpaceCommandPipeline46(
        SpaceCommandBehavior4 behavior1,
        SpaceCommandBehavior5 behavior2,
        SpaceCommandBehavior6 behavior3,
        IRequestHandler<SpaceCommand, Unit> handler
    )
    {
        var piplineBuilder = new PipelineBuilder<SpaceCommand, Unit>();
        _pipeline = piplineBuilder
            .AddBehavior(behavior1)
            .AddBehavior(behavior2)
            .AddBehavior(behavior3)
            .SetHandler(handler)
            .Build();
    }

    public Task<Result<Unit>> HandleAsync(SpaceCommand command, CancellationToken cancellation) =>
        _pipeline.HandleAsync(command, cancellation);
    
}