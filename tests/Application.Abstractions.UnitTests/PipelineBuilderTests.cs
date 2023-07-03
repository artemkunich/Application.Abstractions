using System.Text;
using Akunich.Application.Abstractions;
using FluentAssertions;
using Xunit;

namespace Application.Abstractions.UnitTests;

public class PipelineBuilderTests
{
    [Fact]
    public async Task TestOrderOfBehaviorsAndHandler()
    {
        var resultBuilder = new StringBuilder();
        var behavior1 = new TestCommandBehavior(resultBuilder, "1 ", " 7");
        var behavior2 = new TestCommandBehavior(resultBuilder, "2 ", " 6");
        var behavior3 = new TestCommandBehavior(resultBuilder, "3 ", " 5");
        var handler = new TestCommandHandler(resultBuilder, "4");

        var piplineBuilder = new PipelineBuilder<TestCommand, Unit>();
        var pipeline = piplineBuilder
            .AddBehavior(behavior1)
            .AddBehavior(behavior2)
            .AddBehavior(behavior3)
            .SetHandler(handler)
            .Build();

        var testCommandValue = "Test value";
        var testCommand = new TestCommand
        {
            Value = testCommandValue
        };

        await pipeline.HandleAsync(testCommand, default);

        testCommand.Value.Should().Be(testCommandValue);
        resultBuilder.ToString().Should().Be("1 2 3 4 5 6 7");
    }
    
    [Fact]
    public async Task TestOrderOfBehaviorsAndPipeline()
    {
        var resultBuilder = new StringBuilder();
        var behavior4 = new TestCommandBehavior(resultBuilder, "4 ", " 10");
        var behavior5 = new TestCommandBehavior(resultBuilder, "5 ", " 9");
        var behavior6 = new TestCommandBehavior(resultBuilder, "6 ", " 8");
        var handler = new TestCommandHandler(resultBuilder, "7");

        var piplineBuilder = new PipelineBuilder<TestCommand, Unit>();
        var pipeline = piplineBuilder
            .AddBehavior(behavior4)
            .AddBehavior(behavior5)
            .AddBehavior(behavior6)
            .SetHandler(handler)
            .Build();

        var testCommandValue = "Test value";
        var testCommand = new TestCommand
        {
            Value = testCommandValue
        };

        var behavior1 = new TestCommandBehavior(resultBuilder, "1 ", " 13");
        var behavior2 = new TestCommandBehavior(resultBuilder, "2 ", " 12");
        var behavior3 = new TestCommandBehavior(resultBuilder, "3 ", " 11");
        piplineBuilder = new PipelineBuilder<TestCommand, Unit>();
        pipeline = piplineBuilder
            .AddBehavior(behavior1)
            .AddBehavior(behavior2)
            .AddBehavior(behavior3)
            .SetHandler(pipeline)
            .Build();
        
        await pipeline.HandleAsync(testCommand, default);

        testCommand.Value.Should().Be(testCommandValue);
        resultBuilder.ToString().Should().Be("1 2 3 4 5 6 7 8 9 10 11 12 13");
    }
}


internal class TestCommandBehavior : IPipelineBehavior<TestCommand,Unit>
{
    private StringBuilder _resultBuilder;
    private string _appendBefore;
    private string _appendAfter;
    
    public TestCommandBehavior(StringBuilder resultBuilder, string appendBefore, string appendAfter)
    {
        _resultBuilder = resultBuilder;
        _appendBefore = appendBefore;
        _appendAfter = appendAfter;
    }

    public async Task<Result<Unit>> HandleAsync(TestCommand request, CancellationToken cancellation, NextDelegate<Unit> nextAsync)
    {
        _resultBuilder.Append(_appendBefore);
        var result = await nextAsync();
        _resultBuilder.Append(_appendAfter);
        return result;
    }
}

internal class TestCommandHandler : IRequestHandler<TestCommand,Unit>
{
    private StringBuilder _resultBuilder;
    private string _appendString;
    
    public TestCommandHandler(StringBuilder resultBuilder, string appendString)
    {
        _resultBuilder = resultBuilder;
        _appendString = appendString;
    }
    
    public Task<Result<Unit>> HandleAsync(TestCommand request, CancellationToken cancellation)
    {
        _resultBuilder.Append(_appendString);
        return Task.FromResult<Result<Unit>>(Unit.Value);
    }
}

internal class TestCommand : IRequest<Unit>
{
    public string Value { get; set; }
}