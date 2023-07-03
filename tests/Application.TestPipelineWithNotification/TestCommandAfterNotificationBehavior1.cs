using Application.TestUtils;

namespace Application.TestPipelineWithNotification;

public class TestCommandAfterNotificationBehavior1 : AbstractCommandBehavior<TestCommandAfterNotification>
{
    public TestCommandAfterNotificationBehavior1(
        IList<int> resultBuilder, 
        TestConfiguration testConfiguration) : base(
        resultBuilder, 
        testConfiguration.GetBehaviorBefore(4), 
        testConfiguration.GetBehaviorAfter(4))
    {
    }
}