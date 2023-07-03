using Akunich.Application.Abstractions;

namespace Application.TestPipeline;

public class TestCommandPipeline46 : IPipeline<TestCommand, Unit>
{
    private IPipeline<TestCommand, Unit> _pipeline;
    
    public TestCommandPipeline46(
        TestCommandBehavior4 behavior1,
        TestCommandBehavior5 behavior2,
        TestCommandBehavior6 behavior3,
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