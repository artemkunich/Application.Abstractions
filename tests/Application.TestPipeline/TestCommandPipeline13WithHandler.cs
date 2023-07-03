using Akunich.Application.Abstractions;

namespace Application.TestPipeline;

public class TestCommandPipeline13WithHandler : IPipeline<TestCommand, Unit>
{
    private IPipeline<TestCommand, Unit> _pipeline;
    
    public TestCommandPipeline13WithHandler(
        TestCommandBehavior1 behavior1,
        TestCommandBehavior2 behavior2,
        TestCommandBehavior3 behavior3,
        IRequestHandler<TestCommand, Unit> handler
    )
    {
        var piplineBuilder = new PipelineBuilder<TestCommand, Unit>();
        _pipeline = piplineBuilder
            .AddBehavior(behavior1)
            .AddBehavior(behavior2)
            .AddBehavior(behavior3)
            .SetHandler(handler)
            .Build();
    }

    public Task<Result<Unit>> HandleAsync(TestCommand request, CancellationToken cancellation) =>
        _pipeline.HandleAsync(request, cancellation);
    
}