using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Akunich.Application.Abstractions;

public delegate Task<Result<TResponse>> HandlerDelegate<TResponse>();

public interface IPipelineBehavior<in TRequest, TResponse>
{
    Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken cancellation, Func<Task<Result<TResponse>>> nextAsync);
}