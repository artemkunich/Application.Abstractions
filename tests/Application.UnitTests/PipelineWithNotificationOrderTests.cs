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
        var services = new ServiceCollection();
        services
            .AddScoped<StringBuilder>()
            .AddMediator(mconf => mconf
                .ConfigurePipeline<SpaceCommand, Unit>(pconf => pconf
                    .AddBehavior<SpaceCommandBehavior1>()
                    .AddBehavior<SpaceCommandBehavior2>()
                    .AddBehavior<SpaceCommandBehavior3>()
                    .SetHandler<SpaceCommandHandler>()
                )
                .ConfigurePipeline<UnderscoreCommand,Unit>(pconf => pconf
                    .SetHandler<UnderscoreCommandHandler>()
                )
                .BindNotification<SpaceNotification, UnderscoreCommand, Unit>(n => new UnderscoreCommand(n.BehaviorsCount)
                {
                    Value = n.Value
                })
            );

        var serviceProvider = services.BuildServiceProvider();      
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var spaceCommandValue = "Space value";
        var spaceCommand = new SpaceCommand(3)
        {
            Value = spaceCommandValue,
            RaiseNotification = true
        };

        await mediator.DispatchAsync<SpaceCommand,Unit>(spaceCommand, default);

        spaceCommand.Value.Should().Be(spaceCommandValue);
        serviceProvider.GetRequiredService<StringBuilder>().ToString().Should()
            .BeEquivalentTo(" 1 2 3_4_5 6 7 ");
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task TestOrderOfBehaviorsWithPipeline(bool isReverse)
    {
        var services = new ServiceCollection();
        services
            .AddScoped<StringBuilder>()
            .AddMediator(mconf => {
                mconf.ConfigurePipeline<SpaceCommand, Unit>(pconf => pconf
                    .AddBehavior<SpaceCommandBehavior1>()
                    .AddBehavior<SpaceCommandBehavior2>()
                    .AddBehavior<SpaceCommandBehavior3>()
                    .SetHandler<SpaceCommandHandler>()
                );


                mconf.ConfigurePipeline<SpaceCommand, Unit>("reverse", pconf => pconf
                    .AddBehavior<SpaceCommandBehavior3>()
                    .AddBehavior<SpaceCommandBehavior2>()
                    .AddBehavior<SpaceCommandBehavior1>()
                    .SetHandler<SpaceCommandHandler>()
                );

                mconf.ConfigurePipeline<UnderscoreCommand, Unit>(pconf => pconf
                    .AddBehavior<UnderscoreCommandBehavior4>()
                    .AddBehavior<UnderscoreCommandBehavior5>()
                    .AddBehavior<UnderscoreCommandBehavior6>()
                    .SetHandler<UnderscoreCommandHandler>()
                );

                mconf.ConfigurePipeline<UnderscoreCommand, Unit>("reverse", pconf => pconf
                    .AddBehavior<UnderscoreCommandBehavior6>()
                    .AddBehavior<UnderscoreCommandBehavior5>()
                    .AddBehavior<UnderscoreCommandBehavior4>()
                    .SetHandler<UnderscoreCommandHandler>()
                );

                if (isReverse)
                {
                    mconf.BindNotification<SpaceNotification, UnderscoreCommand, Unit>("reverse", n =>
                        new UnderscoreCommand(n.BehaviorsCount)
                        {
                            Value = n.Value
                        }
                    );
                } else {
                    mconf.BindNotification<SpaceNotification, UnderscoreCommand, Unit>(n =>
                        new UnderscoreCommand(n.BehaviorsCount)
                        {
                            Value = n.Value
                        }
                    );
                }

                return mconf;
            });

        var serviceProvider = services.BuildServiceProvider();
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var spaceCommandValue = "Space value";
        var spaceCommand = new SpaceCommand(6)
        {
            Value = spaceCommandValue,
            RaiseNotification = true
        };

        await mediator.DispatchAsync<SpaceCommand, Unit>(spaceCommand, default);

        if (isReverse)
        {
            spaceCommand.Value.Should().Be(spaceCommandValue);
            var stringBuilder = serviceProvider.GetRequiredService<StringBuilder>();
            stringBuilder.ToString().Should()
                .BeEquivalentTo(" 1 2 3_6_5_4_7_10_9_8_11 12 13 ");

            stringBuilder.Clear();
            await mediator.DispatchAsync<SpaceCommand, Unit>("reverse", spaceCommand, default);
            stringBuilder.ToString().Should()
                .BeEquivalentTo(" 3 2 1_6_5_4_7_10_9_8_13 12 11 ");
        }
        else
        {
            spaceCommand.Value.Should().Be(spaceCommandValue);
            var stringBuilder = serviceProvider.GetRequiredService<StringBuilder>();
            stringBuilder.ToString().Should()
                .BeEquivalentTo(" 1 2 3_4_5_6_7_8_9_10_11 12 13 ");

            stringBuilder.Clear();
            await mediator.DispatchAsync<SpaceCommand, Unit>("reverse", spaceCommand, default);
            stringBuilder.ToString().Should()
                .BeEquivalentTo(" 3 2 1_4_5_6_7_8_9_10_13 12 11 ");
        }
    }
}
