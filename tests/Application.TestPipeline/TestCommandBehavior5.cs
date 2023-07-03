using Application.TestUtils;

namespace Application.TestPipeline;

public class TestCommandBehavior5 : AbstractCommandBehavior<TestCommand>
{
    public TestCommandBehavior5(
        IList<int> resultBuilder, 
        TestConfiguration testConfiguration) : base(
        resultBuilder, 
        testConfiguration.GetBehaviorBefore(5), 
        testConfiguration.GetBehaviorAfter(5))
    {
    }
}