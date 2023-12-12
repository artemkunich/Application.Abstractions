using Application.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace FluentMediator.Internal;

internal interface IPipeline<in TRequest, TResponse>
{
    Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken cancellation);
}