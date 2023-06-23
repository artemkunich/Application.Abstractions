using System.Threading;
using System.Threading.Tasks;

namespace Akunich.Application.Abstractions;

public interface IPipeline<in TRequest, TResponse>
{
    Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken cancellation);
}