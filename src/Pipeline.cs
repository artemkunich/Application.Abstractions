using System.Threading;
using System.Threading.Tasks;

namespace Akunich.Application.Abstractions;

public abstract class Pipeline<TRequest, TResponse> : IPipeline<TRequest, TResponse> 
    where TRequest : IRequest<TResponse>
{
    private readonly IPipeline<TRequest, TResponse> _pipeline;

    public Pipeline(
        IRequestHandler<TRequest,TResponse> handler,
        params IPipelineBehavior<TRequest, TResponse>[] behaviors)
    {
        var pipelineBuilder = new PipelineBuilder<TRequest, TResponse>();
        pipelineBuilder.SetHandler(handler);

        foreach (var behavior in behaviors)
        {
            pipelineBuilder.AddBehavior(behavior);
        }

        _pipeline = pipelineBuilder.Build();
    }
    
    public Pipeline(
        IPipeline<TRequest,TResponse> pipeline,
        params IPipelineBehavior<TRequest, TResponse>[] behaviors)
    {
        var pipelineBuilder = new PipelineBuilder<TRequest, TResponse>();
        pipelineBuilder.SetHandler(pipeline);

        foreach (var behavior in behaviors)
        {
            pipelineBuilder.AddBehavior(behavior);
        }

        _pipeline = pipelineBuilder.Build();
    }
    
    public Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken cancellation) =>
        _pipeline.HandleAsync(request, cancellation);
}

