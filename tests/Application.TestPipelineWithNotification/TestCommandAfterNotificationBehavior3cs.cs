using Application.TestUtils;

namespace Application.TestPipelineWithNotification;

public class TestCommandAfterNotificationBehavior3 : AbstractCommandBehavior<TestCommandAfterNotification>
{
    public TestCommandAfterNotificationBehavior3(
        IList<int> resultBuilder, 
        TestConfiguration testConfiguration) : base(
        resultBuilder, 
        testConfiguration.GetBehaviorBefore(6), 
        testConfiguration.GetBehaviorAfter(6))
    {
    }
}