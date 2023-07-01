using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Akunich.Application.Abstractions.Internal;

internal sealed class RequestDispatcher : IRequestDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public RequestDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task<Result<TResult>> DispatchAsync<TRequest, TResult>(TRequest request, CancellationToken cancellation = new CancellationToken()) where TRequest : IRequest<TResult>
    {
        var requestHandler = _serviceProvider.GetRequiredService<IRequestHandler<TRequest, TResult>>();
        return requestHandler.HandleAsync(request, cancellation);
    }
}