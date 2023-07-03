using Akunich.Application.Abstractions;
using Application.TestPipelineWithNotification;
using Application.TestUtils;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Application.Abstractions.UnitTests;

public class PipelineWithNotificationOrderTests
{
    [Fact]
    public async Task TestOrderOfBehaviorsAndHandler()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection
            .AddScoped<IList<int>, List<int>>()
            .AddScoped(sp => new TestConfiguration(){BehaviorsCount = 3})
            .AddNotificationDispatcher()
            .AddApplication(typeof(TestCommand).Assembly)
            .AddNotificationMediator<TestNotification, TestCommandAfterNotification>(n => new TestCommandAfterNotification
            {
                ValueFromNotification = n.HandlerOrder
            });
        var services = serviceCollection.BuildServiceProvider();
        
        var pipeline = services.GetRequiredService<TestCommandPipelineWithHandler>();

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
            .AddScoped<IList<int>, List<int>>()
            .AddScoped(sp => new TestConfiguration(){BehaviorsCount = 6})
            .AddNotificationDispatcher()
            .AddApplication(typeof(TestCommand).Assembly)
            .AddNotificationMediator<TestNotification, TestCommandAfterNotification, TestCommandAfterNotificationPipeline>(n => new TestCommandAfterNotification
            {
                ValueFromNotification = n.HandlerOrder
            });
        var services = serviceCollection.BuildServiceProvider();
        
        var pipeline = services.GetRequiredService<TestCommandPipelineWithHandler>();

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
    
    // [Fact]
    // public async Task TestOrderOfBehaviorsAndPipeline()
    // {
    //     var services = ContainerFactory.CreateContainer(typeof(TestCommand7));
    //     var pipeline = services.GetRequiredService<TestCommand7Pipeline_1_3>();
    //
    //     var testCommandValue = "Test value";
    //     var testCommand = new TestCommand7
    //     {
    //         Value = testCommandValue
    //     };
    //
    //     await pipeline.HandleAsync(testCommand, default);
    //
    //     testCommand.Value.Should().Be(testCommandValue);
    //     services.GetRequiredService<IList<int>>().ToArray().Should()
    //         .BeEquivalentTo(new []{ 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 });
    // }
}
