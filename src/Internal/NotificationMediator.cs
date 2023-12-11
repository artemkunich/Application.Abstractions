using System.Threading;
using System.Threading.Tasks;

namespace Akunich.Application.Abstractions.Internal;

internal sealed class NotificationMediator<TNotification, TRequest, TResponse> : INotificationHandler<TNotification> 
    where TNotification : INotification 
    where TRequest : IRequest<TResponse>
{
    private readonly MapNotificationDelegate<TNotification, TRequest, TResponse> _mapNotification;
    private readonly Pipeline<TRequest,TResponse> _pipeline;
    
    public NotificationMediator(MapNotificationDelegate<TNotification, TRequest, TResponse> mapNotification, Pipeline<TRequest, TResponse> pipeline)
    {
        _pipeline = pipeline;
        _mapNotification = mapNotification;

    }

    public async Task<Result> HandleAsync(TNotification notification, CancellationToken cancellation)
    {
        var command = _mapNotification(notification);
        return await _pipeline.HandleAsync(command, cancellation);
    }
}