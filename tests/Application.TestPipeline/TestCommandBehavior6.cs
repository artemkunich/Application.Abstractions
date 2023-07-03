using Application.TestUtils;

namespace Application.TestPipeline;

public class TestCommandBehavior6 : AbstractCommandBehavior<TestCommand>
{
    public TestCommandBehavior6(
        IList<int> resultBuilder, 
        TestConfiguration testConfiguration) : base(
        resultBuilder, 
        testConfiguration.GetBehaviorBefore(6), 
        testConfiguration.GetBehaviorAfter(6))
    {
    }
}