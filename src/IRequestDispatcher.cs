using System.Threading;
using System.Threading.Tasks;

namespace Akunich.Application.Abstractions;

public interface IRequestDispatcher
{
    Task<Result<TResult>> DispatchAsync<TRequest, TResult>(TRequest request, CancellationToken cancellation = default) where TRequest: IRequest<TResult>;
}