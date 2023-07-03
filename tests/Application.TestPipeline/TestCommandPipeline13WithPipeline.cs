using Akunich.Application.Abstractions;

namespace Application.TestPipeline;

public class TestCommandPipeline13WithPipeline : IPipeline<TestCommand, Unit>
{
    private IPipeline<TestCommand, Unit> _pipeline;
    
    public TestCommandPipeline13WithPipeline(
        TestCommandBehavior1 behavior1,
        TestCommandBehavior2 behavior2,
        TestCommandBehavior3 behavior3,
        TestCommandPipeline46 pipeline
    )
    {
        var piplineBuilder = new PipelineBuilder<TestCommand, Unit>();
        _pipeline = piplineBuilder
            .AddBehavior(behavior1)
            .AddBehavior(behavior2)
            .AddBehavior(behavior3)
            .SetHandler(pipeline)
            .Build();
    }

    public Task<Result<Unit>> HandleAsync(TestCommand request, CancellationToken cancellation) =>
        _pipeline.HandleAsync(request, cancellation);
    
}