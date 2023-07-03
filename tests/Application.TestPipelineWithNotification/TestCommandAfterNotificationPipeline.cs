using Akunich.Application.Abstractions;

namespace Application.TestPipelineWithNotification;

public class TestCommandAfterNotificationPipeline : IPipeline<TestCommandAfterNotification, Unit>
{
    private IPipeline<TestCommandAfterNotification, Unit> _pipeline;
    
    public TestCommandAfterNotificationPipeline(
        TestCommandAfterNotificationBehavior1 behavior1,
        TestCommandAfterNotificationBehavior2 behavior2,
        TestCommandAfterNotificationBehavior3 behavior3,
        IRequestHandler<TestCommandAfterNotification, Unit> handler
    )
    {
        var piplineBuilder = new PipelineBuilder<TestCommandAfterNotification, Unit>();
        _pipeline = piplineBuilder
            .AddBehavior(behavior1)
            .AddBehavior(behavior2)
            .AddBehavior(behavior3)
            .SetHandler(handler)
            .Build();
    }

    public Task<Result<Unit>> HandleAsync(TestCommandAfterNotification request, CancellationToken cancellation) =>
        _pipeline.HandleAsync(request, cancellation);
    
}