using System.Threading;
using System.Threading.Tasks;

namespace Akunich.Application.Abstractions.Internal;

internal interface IPipeline<in TRequest, TResponse>
{
    Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken cancellation);
}