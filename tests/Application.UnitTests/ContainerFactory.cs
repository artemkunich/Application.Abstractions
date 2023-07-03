using Akunich.Application.Abstractions;

using Microsoft.Extensions.DependencyInjection;

namespace Application.Abstractions.UnitTests;

public class ContainerFactory
{
    public static IServiceProvider CreateContainer(Type commandType)
    {
        var services = new ServiceCollection();
        services
            .AddScoped<IList<int>,List<int>>()
            .AddApplication(commandType.Assembly);
        return services.BuildServiceProvider();
    }
}