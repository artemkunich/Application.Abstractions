using Application.Abstractions;

namespace FluentMediator.Internal;

internal class PipelineConfiguration<TRequest, TResponse> : 
    IPipelineConfiguration<TRequest,TResponse>, IPipelineConfigurationCompleted<TRequest,TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly MediatorTypesStore _behaviorStore;

    public PipelineConfiguration(MediatorTypesStore behaviorStore)
    {
        _behaviorStore = behaviorStore;
    }

    public IPipelineConfiguration<TRequest, TResponse> AddBehavior<TBehavior>() where TBehavior : class, IPipelineBehavior<TRequest, TResponse>
    {
        _behaviorStore.AddBehavior<TRequest, TResponse, TBehavior>();
        return this;
    }

    public IPipelineConfigurationCompleted<TRequest, TResponse> SetHandler<THandler>() where THandler : class, IHandler<TRequest, TResponse>
    {
        _behaviorStore.AddHandler<TRequest, TResponse, THandler>();
        return this;
    }
}
