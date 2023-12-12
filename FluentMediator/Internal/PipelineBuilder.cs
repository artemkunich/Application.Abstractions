using Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FluentMediator.Internal;

internal sealed class PipelineBuilder<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    public static PipelineBuilder<TRequest, TResponse> Create() => new();

    private readonly List<object> _behaviors;
    private Func<TRequest, CancellationToken, Task<Result<TResponse>>> _handler;

    public PipelineBuilder()
    {
        _behaviors = new List<object>();
        _handler = (_, _) => throw new NotImplementedException();
    }

    public PipelineBuilder<TRequest, TResponse> AddBehavior(Func<TRequest, CancellationToken,
        NextDelegate<TResponse>, Task<Result<TResponse>>> behavior)
    {
        _behaviors.Add(behavior);
        return this;
    }

    public PipelineBuilder<TRequest, TResponse> AddBehavior(Func<TRequest, CancellationToken,
        NextDelegate<TRequest, TResponse>, Task<Result<TResponse>>> behavior)
    {
        _behaviors.Add(behavior);
        return this;
    }

    public PipelineBuilder<TRequest, TResponse> SetHandler(Func<TRequest, CancellationToken,
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
                next = (request, cancellation) => converter(request, cancellation, (r, c) => nextNext(r, c));
            else if (behavior is Func<TRequest, CancellationToken, NextDelegate<TResponse>, Task<Result<TResponse>>> shortBehavior)
                next = (request, cancellation) => shortBehavior(request, cancellation, () => nextNext(request, cancellation));
            else
                throw new InvalidOperationException("Unexpected type of behavior");
        }

        return new PipelineStub<TRequest, TResponse>(next);
    }

    private class PipelineStub<TReq, TRes> : IPipeline<TReq, TRes>
    {
        private readonly Func<TReq, CancellationToken, Task<Result<TRes>>> _handle;

        public PipelineStub(Func<TReq, CancellationToken, Task<Result<TRes>>> handle)
        {
            _handle = handle;
        }

        public Task<Result<TRes>> HandleAsync(TReq request, CancellationToken cancellation) =>
            _handle(request, cancellation);
    }
}

public static class PipelineBundlerExtensions
{
    internal static PipelineBuilder<TRequest, TResponse> AddBehavior<TRequest, TResponse>(this PipelineBuilder<TRequest, TResponse> builder, IPipelineBehavior<TRequest, TResponse> behavior) where TRequest : IRequest<TResponse>
    {
        builder.AddBehavior(behavior.HandleAsync);
        return builder;
    }

    internal static PipelineBuilder<TRequest, TResponse> SetHandler<TRequest, TResponse>(this PipelineBuilder<TRequest, TResponse> builder, IHandler<TRequest, TResponse> handler) where TRequest : IRequest<TResponse>
    {
        builder.SetHandler(handler.HandleAsync);
        return builder;
    }

    internal static PipelineBuilder<TRequest, TResponse> SetHandler<TRequest, TResponse>(this PipelineBuilder<TRequest, TResponse> builder, IPipeline<TRequest, TResponse> pipeline) where TRequest : IRequest<TResponse>
    {
        builder.SetHandler(pipeline.HandleAsync);
        return builder;
    }
}