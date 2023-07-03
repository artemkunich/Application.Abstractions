using Application.TestUtils;

namespace Application.TestPipeline;

public class TestCommandBehavior4 : AbstractCommandBehavior<TestCommand>
{
    public TestCommandBehavior4(
        IList<int> resultBuilder, 
        TestConfiguration testConfiguration) : base(
        resultBuilder, 
        testConfiguration.GetBehaviorBefore(4), 
        testConfiguration.GetBehaviorAfter(4))
    {
    }
}