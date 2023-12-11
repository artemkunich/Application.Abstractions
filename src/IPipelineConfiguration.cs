using System;
using System.Collections.Generic;
using System.Text;

namespace Akunich.Application.Abstractions;

public interface IPipelineConfiguration<TRequest,TResponse> where TRequest : IRequest<TResponse>
{
    IPipelineConfiguration<TRequest, TResponse> AddBehavior<TBehavior>() 
        where TBehavior : class, IPipelineBehavior<TRequest, TResponse>;

    IPipelineConfigurationCompleted<TRequest, TResponse> SetHandler<THandler>()
        where THandler : class, IRequestHandler<TRequest, TResponse>;
}

public interface IPipelineConfigurationCompleted<TRequest, TResponse>
{
}
