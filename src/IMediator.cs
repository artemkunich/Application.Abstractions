using System.Threading.Tasks;
using System.Threading;

namespace Application.Abstractions;

public interface IMediator
{
    Task<Result<TResult>> SendAsync<TRequest, TResult>(TRequest request, CancellationToken cancellation = default) where TRequest : IRequest<TResult>;

    Task<Result<TResult>> SendAsync<TRequest, TResult>(object key, TRequest request, CancellationToken cancellation = default) where TRequest : IRequest<TResult>;


    Task<Result> PublishAsync<TNotification>(TNotification request, CancellationToken cancellation = default) where TNotification : INotification;
}