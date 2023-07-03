using Akunich.Application.Abstractions;
using Application.TestPipeline;
using Application.TestUtils;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Application.Abstractions.UnitTests;

public class PipelineOrderTests
{
    [Fact]
    public async Task TestOrderOfBehaviorsAndHandler()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection
            .AddScoped(sp => new TestConfiguration
            {
                BehaviorsCount = 3
            })
            .AddScoped<IList<int>,List<int>>()
            .AddApplication(typeof(TestCommand).Assembly);
        var services = serviceCollection.BuildServiceProvider();
        
        var pipeline = services.GetRequiredService<TestCommandPipeline13WithHandler>();

        var testCommandValue = "Test value";
        var testCommand = new TestCommand
        {
            Value = testCommandValue
        };

        await pipeline.HandleAsync(testCommand, default);

        testCommand.Value.Should().Be(testCommandValue);
        services.GetRequiredService<IList<int>>().ToArray().Should()
            .BeEquivalentTo(new []{ 1, 2, 3, 4, 5, 6, 7 });
    }
 
    [Fact]
    public async Task TestOrderOfBehaviorsAndPipeline()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection
            .AddScoped(sp => new TestConfiguration
            {
                BehaviorsCount = 6
            })
            .AddScoped<IList<int>,List<int>>()
            .AddApplication(typeof(TestCommand).Assembly);
        var services = serviceCollection.BuildServiceProvider();
        var pipeline = services.GetRequiredService<TestCommandPipeline13WithPipeline>();

        var testCommandValue = "Test value";
        var testCommand = new TestCommand
        {
            Value = testCommandValue
        };

        await pipeline.HandleAsync(testCommand, default);

        testCommand.Value.Should().Be(testCommandValue);
        services.GetRequiredService<IList<int>>().ToArray().Should()
            .BeEquivalentTo(new []{ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 });
    }
}
