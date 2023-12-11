using System.Threading.Tasks;
using System.Threading;

namespace Akunich.Application.Abstractions;

public interface IMediator
{
    Task<Result<TResult>> DispatchAsync<TRequest, TResult>(TRequest request, CancellationToken cancellation = default) where TRequest : IRequest<TResult>;

    Task<Result<TResult>> DispatchAsync<TRequest, TResult>(object key, TRequest request, CancellationToken cancellation = default) where TRequest : IRequest<TResult>;

    Task<Result> DispatchAsync<TNotification>(TNotification request, CancellationToken cancellation = default) where TNotification : INotification;
}