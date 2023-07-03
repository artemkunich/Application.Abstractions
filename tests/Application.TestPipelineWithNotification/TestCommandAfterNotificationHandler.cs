using Akunich.Application.Abstractions;

namespace Application.TestPipelineWithNotification;

public class TestCommandAfterNotificationHandler : IRequestHandler<TestCommandAfterNotification, Unit>
{
    private readonly IList<int> _resultBuilder;

    public TestCommandAfterNotificationHandler(IList<int> resultBuilder)
    {
        _resultBuilder = resultBuilder;
    }

    public Task<Result<Unit>> HandleAsync(TestCommandAfterNotification request, CancellationToken cancellation)
    {
        _resultBuilder.Add(request.ValueFromNotification);

        return Task.FromResult(Result.Create(Unit.Value));
    }
}