using System;
using System.Threading;
using System.Threading.Tasks;

namespace Akunich.Application.Abstractions.Internal;

internal class Pipeline<TRequest, TResponse> : IPipeline<TRequest, TResponse>
{
    private readonly Func<TRequest, CancellationToken, Task<Result<TResponse>>> _handle;
    
    public Pipeline(Func<TRequest,CancellationToken,Task<Result<TResponse>>> handle)
    {
        _handle = handle;
    }

    public Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken cancellation) => 
        _handle(request, cancellation);
}