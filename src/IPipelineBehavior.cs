using System.Threading;
using System.Threading.Tasks;

namespace Akunich.Application.Abstractions;

public delegate Task<Result<TResponse>> NextDelegate<in TRequest,TResponse>(TRequest request, CancellationToken cancellation);
public delegate Task<Result<TResponse>> NextDelegate<TResponse>();

public interface IPipelineBehavior<in TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken cancellation, NextDelegate<TResponse> nextAsync);
}