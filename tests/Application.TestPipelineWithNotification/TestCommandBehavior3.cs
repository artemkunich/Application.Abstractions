using Application.TestUtils;

namespace Application.TestPipelineWithNotification;

public class TestCommandBehavior3 : AbstractCommandBehavior<TestCommand>
{
    public TestCommandBehavior3(
        IList<int> resultBuilder, 
        TestConfiguration testConfiguration) : base(
        resultBuilder, 
        testConfiguration.GetBehaviorBefore(3), 
        testConfiguration.GetBehaviorAfter(3))
    {
    }
}