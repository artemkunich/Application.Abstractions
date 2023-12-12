using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.Extensions.DependencyInjection;
using Application.Abstractions;

namespace FluentMediator.Internal;

internal class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task<Result<TResult>> SendAsync<TRequest, TResult>(TRequest request, CancellationToken cancellation = default) where TRequest : IRequest<TResult>
    {
        var requestHandler = _serviceProvider.GetRequiredService<Pipeline<TRequest, TResult>>();
        return requestHandler.HandleAsync(request, cancellation);
    }

    public Task<Result<TResult>> SendAsync<TRequest, TResult>(object key, TRequest request, CancellationToken cancellation = default) where TRequest : IRequest<TResult>
    {
        var requestHandler = _serviceProvider.GetRequiredKeyedService<Pipeline<TRequest, TResult>>(key);
        return requestHandler.HandleAsync(request, cancellation);
    }

    public async Task<Result> PublishAsync<TNotification>(TNotification notification, CancellationToken cancellation = default) where TNotification : INotification
    {
        var eventHandlers = _serviceProvider.GetServices<INotificationHandler<TNotification>>();
        foreach (var eventHandler in eventHandlers)
        {
            var result = await eventHandler.HandleAsync(notification, cancellation);
            if (result.IsFailure)
                return result;
        }

        return Result.Create();
    }
}
