using System.Text;
using Akunich.Application.Abstractions;
using Application.Space;
using Application.Underscore;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Application.Abstractions.UnitTests;

public class PipelineWithNotificationOrderTests
{
    [Fact]
    public async Task TestOrderOfBehaviorsWithHandler()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection
            .AddScoped<StringBuilder>()
            .AddNotificationDispatcher()
            .AddApplication(typeof(SpaceCommand).Assembly)
            .AddApplication(typeof(UnderscoreCommand).Assembly)
            .BindNotification<SpaceNotification, UnderscoreCommand, Unit>(n => new UnderscoreCommand(n.BehaviorsCount)
            {
                Value = n.Value
            });
        var services = serviceCollection.BuildServiceProvider();
        
        var pipeline = services.GetRequiredService<SpaceCommandPipeline13WithHandler>();

        var spaceCommandValue = "Space value";
        var spaceCommand = new SpaceCommand(3)
        {
            Value = spaceCommandValue,
            RaiseNotification = true
        };

        await pipeline.HandleAsync(spaceCommand, default);

        spaceCommand.Value.Should().Be(spaceCommandValue);
        services.GetRequiredService<StringBuilder>().ToString().Should()
            .BeEquivalentTo(" 1 2 3_4_5 6 7 ");
    }
 
    [Fact]
    public async Task TestOrderOfBehaviorsWithPipeline()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection
            .AddScoped<StringBuilder>()
            .AddNotificationDispatcher()
            .AddApplication(typeof(SpaceCommand).Assembly)
            .AddApplication(typeof(UnderscoreCommand).Assembly)
            .BindNotification<SpaceNotification, UnderscoreCommand, Unit, UnderscoreCommandPipeline46>(n => 
                new UnderscoreCommand(n.BehaviorsCount)
                {
                    Value = n.Value
                });
        var services = serviceCollection.BuildServiceProvider();
        
        var pipeline = services.GetRequiredService<SpaceCommandPipeline13WithHandler>();

        var spaceCommandValue = "Space value";
        var spaceCommand = new SpaceCommand(6)
        {
            Value = spaceCommandValue,
            RaiseNotification = true
        };

        await pipeline.HandleAsync(spaceCommand, default);

        spaceCommand.Value.Should().Be(spaceCommandValue);
        services.GetRequiredService<StringBuilder>().ToString().Should()
            .BeEquivalentTo(" 1 2 3_4_5_6_7_8_9_10_11 12 13 ");
    }
}
