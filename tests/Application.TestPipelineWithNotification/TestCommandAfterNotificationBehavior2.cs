using Application.TestUtils;

namespace Application.TestPipelineWithNotification;

public class TestCommandAfterNotificationBehavior2 : AbstractCommandBehavior<TestCommandAfterNotification>
{
    public TestCommandAfterNotificationBehavior2(
        IList<int> resultBuilder, 
        TestConfiguration testConfiguration) : base(
        resultBuilder, 
        testConfiguration.GetBehaviorBefore(5), 
        testConfiguration.GetBehaviorAfter(5))
    {
    }
}