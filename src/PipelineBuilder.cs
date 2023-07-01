using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Akunich.Application.Abstractions.Internal;

namespace Akunich.Application.Abstractions;

public sealed class PipelineBuilder<TRequest,TResponse> : IPipelineBuilder<TRequest,TResponse> where TRequest : IRequest<TResponse>
{
    public static PipelineBuilder<TRequest, TResponse> Create() => new();
    
    private readonly List<object> _behaviors;
    private Func<TRequest, CancellationToken, Task<Result<TResponse>>> _handler;

    public PipelineBuilder()
    {
        _behaviors = new List<object>();
        _handler = (_,_) => throw new NotImplementedException();
    }
    
    public IPipelineBuilder<TRequest, TResponse> AddBehavior(Func<TRequest, CancellationToken, 
        NextDelegate<TResponse>, Task<Result<TResponse>>> behavior)
    {
        _behaviors.Add(behavior);
        return this;
    }

    public IPipelineBuilder<TRequest, TResponse> AddBehavior(Func<TRequest, CancellationToken,
        NextDelegate<TRequest, TResponse>, Task<Result<TResponse>>> behavior)
    {
        _behaviors.Add(behavior);
        return this;
    }

    public IPipelineBuilder<TRequest, TResponse> SetHandler(Func<TRequest, CancellationToken,
        Task<Result<TResponse>>> handler)
    {
        _handler = handler;
        return this;
    }

    public IPipeline<TRequest, TResponse> Build()
    {
        var behaviors = _behaviors.ToArray().Reverse();
        var next = _handler;

        foreach (var behavior in behaviors)
        {
            var nextNext = next;

            if (behavior is Func<TRequest, CancellationToken, NextDelegate<TRequest, TResponse>, Task<Result<TResponse>>> converter)
                next = (request, cancellation) => converter(request, cancellation, (r,c) => nextNext(r,c));
            else if (behavior is Func<TRequest, CancellationToken, NextDelegate<TResponse>, Task<Result<TResponse>>> shortBehavior)
                next = (request, cancellation) => shortBehavior(request, cancellation, () => nextNext(request, cancellation));
            else
                throw new InvalidOperationException("Unexpected type of behavior");
        }
        
        return new Pipeline<TRequest, TResponse>(next);
    }
}