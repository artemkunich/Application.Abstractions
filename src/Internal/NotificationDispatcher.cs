using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Akunich.Application.Abstractions.Internal;

internal sealed class NotificationDispatcher : INotificationDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public NotificationDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<Result<Unit>> DispatchAsync<TNotification>(TNotification request, CancellationToken cancellation = default) where TNotification: INotification
    {
        var eventHandlers = _serviceProvider.GetServices<INotificationHandler<TNotification>>();
        foreach (var eventHandler in eventHandlers)
        {
            var result = await eventHandler.HandleAsync(request, cancellation);
            if (result.IsFailure)
                return result;
        }
        
        return Unit.Value;
    }
}