using Akunich.Application.Abstractions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Application.Abstractions.UnitTests;

public class ResolveDispatchers
{
    [Fact]
    public void CanResolveDispatchers()
    {
        //IServiceCollection services = new ServiceCollection();
        //services
        //    .AddRequestDispatcher()
        //    .AddNotificationDispatcher();
        //var serviceProvider = services.BuildServiceProvider();
        //var resolveRequestDispatcher = () => serviceProvider.GetService<IRequestDispatcher>();
        //var resolveNotificationDispatcher = () => serviceProvider.GetService<INotificationDispatcher>();

        //resolveRequestDispatcher.Should().NotThrow();
        //resolveNotificationDispatcher.Should().NotThrow();
    }
}