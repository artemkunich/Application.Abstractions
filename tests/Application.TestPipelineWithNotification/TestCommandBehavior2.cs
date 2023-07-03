using Application.TestUtils;

namespace Application.TestPipelineWithNotification;

public class TestCommandBehavior2 : AbstractCommandBehavior<TestCommand>
{
    public TestCommandBehavior2(
        IList<int> resultBuilder, 
        TestConfiguration testConfiguration) : base(
        resultBuilder, 
        testConfiguration.GetBehaviorBefore(2), 
        testConfiguration.GetBehaviorAfter(2))
    {
    }
}