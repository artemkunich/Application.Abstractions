using System.Text;
using Akunich.Application.Abstractions;
using Application.Space;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Application.Abstractions.UnitTests;

public class PipelineOrderTests
{
    [Fact]
    public async Task TestOrderOfBehaviorsWithHandler()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection
            .AddScoped<StringBuilder>()
            .AddNotificationDispatcher()
            .AddApplication(typeof(SpaceCommand).Assembly);
        var services = serviceCollection.BuildServiceProvider();
        
        var pipeline = services.GetRequiredService<SpaceCommandPipeline13WithHandler>();

        var spaceCommandValue = "Space value";
        var spaceCommand = new SpaceCommand(3)
        {
            Value = spaceCommandValue
        };

        await pipeline.HandleAsync(spaceCommand, default);

        spaceCommand.Value.Should().Be(spaceCommandValue);
        services.GetRequiredService<StringBuilder>().ToString().Should()
            .BeEquivalentTo(" 1 2 3 4 5 6 7 ");
    }
 
    [Fact]
    public async Task TestOrderOfBehaviorsWithPipeline()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection
            .AddScoped<StringBuilder>()
            .AddNotificationDispatcher()
            .AddApplication(typeof(SpaceCommand).Assembly);
        var services = serviceCollection.BuildServiceProvider();
        var pipeline = services.GetRequiredService<SpaceCommandPipeline13WithPipeline46>();

        var spaceCommandValue = "Space value";
        var spaceCommand = new SpaceCommand(6)
        {
            Value = spaceCommandValue
        };

        await pipeline.HandleAsync(spaceCommand, default);

        spaceCommand.Value.Should().Be(spaceCommandValue);
        services.GetRequiredService<StringBuilder>().ToString().Should()
            .BeEquivalentTo(" 1 2 3 4 5 6 7 8 9 10 11 12 13 ");
    }
}
