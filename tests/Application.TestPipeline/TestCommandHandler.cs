using Application.TestUtils;

namespace Application.TestPipeline;

public class TestCommandHandler : AbstractCommandHandler<TestCommand>
{
    public TestCommandHandler(
        IList<int> resultBuilder, 
        TestConfiguration testConfiguration) : base(
        resultBuilder, testConfiguration.GetHandlerValue())
    {
    }
}