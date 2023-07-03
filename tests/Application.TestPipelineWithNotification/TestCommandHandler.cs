using Akunich.Application.Abstractions;
using Application.TestUtils;

namespace Application.TestPipelineWithNotification;

public sealed class TestCommandHandler : IRequestHandler<TestCommand,Unit>
{
    private readonly INotificationDispatcher _notificationDispatcher;
    private readonly TestConfiguration _testConfiguration;
    
    public TestCommandHandler(
        INotificationDispatcher notificationDispatcher, TestConfiguration testConfiguration)
    {
        _notificationDispatcher = notificationDispatcher;
        _testConfiguration = testConfiguration;
    }
    
    public async Task<Result<Unit>> HandleAsync(TestCommand request, CancellationToken cancellation)
    {
        var notification = new TestNotification
        {
            HandlerOrder = _testConfiguration.GetHandlerValue()
        };
        return await _notificationDispatcher.DispatchAsync(notification, cancellation);
    }
}