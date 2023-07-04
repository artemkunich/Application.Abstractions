using Akunich.Application.Abstractions;

namespace Application.Underscore;

public sealed class UnderscoreCommandPipeline46 : IPipeline<UnderscoreCommand, Unit>
{
    private IPipeline<UnderscoreCommand, Unit> _pipeline;
    
    public UnderscoreCommandPipeline46(
        UnderscoreCommandBehavior4 behavior4,
        UnderscoreCommandBehavior5 behavior5,
        UnderscoreCommandBehavior6 behavior6,
        IRequestHandler<UnderscoreCommand, Unit> handler
    )
    {
        var piplineBuilder = new PipelineBuilder<UnderscoreCommand, Unit>();
        _pipeline = piplineBuilder
            .AddBehavior(behavior4)
            .AddBehavior(behavior5)
            .AddBehavior(behavior6)
            .SetHandler(handler)
            .Build();
    }

    public Task<Result<Unit>> HandleAsync(UnderscoreCommand command, CancellationToken cancellation) =>
        _pipeline.HandleAsync(command, cancellation);
    
}