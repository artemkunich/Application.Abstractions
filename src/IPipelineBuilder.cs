using System;
using System.Threading;
using System.Threading.Tasks;

namespace Akunich.Application.Abstractions;

public interface IPipelineBuilder<TRequest,TResponse> where TRequest : IRequest<TResponse>
{
    IPipelineBuilder<TRequest, TResponse> AddBehavior(
        Func<TRequest, CancellationToken, NextDelegate<TResponse>, Task<Result<TResponse>>> behavior);

    IPipelineBuilder<TRequest, TResponse> AddBehavior(
        Func<TRequest, CancellationToken, 
            NextDelegate<TRequest, TResponse>, Task<Result<TResponse>>> behavior);
    IPipelineBuilder<TRequest, TResponse> SetHandler(Func<TRequest, CancellationToken, Task<Result<TResponse>>> handler);

    IPipeline<TRequest, TResponse> Build();
}

public static class PipelineBuilderExtensions
{
    public static IPipelineBuilder<TRequest, TResponse> AddBehavior<TRequest, TResponse>(this IPipelineBuilder<TRequest, TResponse> builder, IPipelineBehavior<TRequest, TResponse> behavior) where TRequest : IRequest<TResponse>
    {
        builder.AddBehavior(behavior.HandleAsync);
        return builder;
    }
    
    public static IPipelineBuilder<TRequest, TResponse> SetHandler<TRequest, TResponse>(this IPipelineBuilder<TRequest, TResponse> builder, IRequestHandler<TRequest, TResponse> handler) where TRequest : IRequest<TResponse>
    {
        builder.SetHandler(handler.HandleAsync);
        return builder;
    }
    
    public static IPipelineBuilder<TRequest, TResponse> SetHandler<TRequest, TResponse>(this IPipelineBuilder<TRequest, TResponse> builder, IPipeline<TRequest, TResponse> pipeline) where TRequest : IRequest<TResponse>
    {
        builder.SetHandler(pipeline.HandleAsync);
        return builder;
    }
}