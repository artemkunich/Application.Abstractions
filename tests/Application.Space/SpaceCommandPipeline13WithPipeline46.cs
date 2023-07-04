using Akunich.Application.Abstractions;

namespace Application.Space;

public class SpaceCommandPipeline13WithPipeline46 : IPipeline<SpaceCommand, Unit>
{
    private IPipeline<SpaceCommand, Unit> _pipeline;
    
    public SpaceCommandPipeline13WithPipeline46(
        SpaceCommandBehavior1 behavior1,
        SpaceCommandBehavior2 behavior2,
        SpaceCommandBehavior3 behavior3,
        SpaceCommandPipeline46 pipeline
    )
    {
        var piplineBuilder = new PipelineBuilder<SpaceCommand, Unit>();
        _pipeline = piplineBuilder
            .AddBehavior(behavior1)
            .AddBehavior(behavior2)
            .AddBehavior(behavior3)
            .SetHandler(pipeline)
            .Build();
    }

    public Task<Result<Unit>> HandleAsync(SpaceCommand command, CancellationToken cancellation) =>
        _pipeline.HandleAsync(command, cancellation);
    
}