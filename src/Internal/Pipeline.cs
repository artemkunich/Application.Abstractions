using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Akunich.Application.Abstractions.Internal;

internal sealed class Pipeline<TRequest, TResponse> : IPipeline<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IPipeline<TRequest, TResponse> _pipeline;

    public Pipeline(
        IRequestHandler<TRequest, TResponse> handler,
        MediatorTypesStore behaviorStore,
        IServiceProvider serviceProvider,
        object key = null)
    {
        var pipelineBuilder = new PipelineBuilder<TRequest, TResponse>();
        pipelineBuilder.SetHandler(handler);

        var behaviors = behaviorStore.GetBehaviors<TRequest,TResponse>();

        foreach (var type in behaviors)
        {
            var behaviorService = key is null 
                ? serviceProvider.GetRequiredService(type) as IPipelineBehavior<TRequest,TResponse>
                : serviceProvider.GetRequiredKeyedService(type, key) as IPipelineBehavior<TRequest, TResponse>;
            pipelineBuilder.AddBehavior(behaviorService);
        }

        _pipeline = pipelineBuilder.Build();
    }

    public Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken cancellation) =>
        _pipeline.HandleAsync(request, cancellation);
}

