using Application.TestUtils;

namespace Application.TestPipelineWithNotification;

public class TestCommandBehavior1 : AbstractCommandBehavior<TestCommand>
{
    public TestCommandBehavior1(
        IList<int> resultBuilder, 
        TestConfiguration testConfiguration) : base(
        resultBuilder, 
        testConfiguration.GetBehaviorBefore(1), 
        testConfiguration.GetBehaviorAfter(1))
    {
    }
}