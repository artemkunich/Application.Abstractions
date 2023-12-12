using Application.Abstractions;

namespace FluentMediator;

public interface IPipelineConfiguration<TRequest,TResponse> where TRequest : IRequest<TResponse>
{
    IPipelineConfiguration<TRequest, TResponse> AddBehavior<TBehavior>() 
        where TBehavior : class, IPipelineBehavior<TRequest, TResponse>;

    IPipelineConfigurationCompleted<TRequest, TResponse> SetHandler<THandler>()
        where THandler : class, IHandler<TRequest, TResponse>;
}

public interface IPipelineConfigurationCompleted<TRequest, TResponse>
{
}
