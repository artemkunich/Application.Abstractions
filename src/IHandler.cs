using System.Threading;
using System.Threading.Tasks;

namespace Application.Abstractions;

public interface IHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken cancellation);
}