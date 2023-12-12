using Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentMediator.Internal;

internal class MediatorTypesStore
{
    private readonly Dictionary<Type, List<Type>> _behaviors;
    private readonly Dictionary<Type, Type> _handlers;
    private readonly Dictionary<Type, Type> _handlerRequests;
    private readonly Dictionary<Type, Type> _handlerResponses;

    public MediatorTypesStore()
    {
        _behaviors = new Dictionary<Type, List<Type>>();
        _handlers = new Dictionary<Type,Type>();
        _handlerRequests = new Dictionary<Type, Type>();
        _handlerResponses = new Dictionary<Type, Type>();
    }

    public void AddBehavior<TRequest,TResponse,TBehavior>() 
        where TRequest : IRequest<TResponse>
        where TBehavior : IPipelineBehavior<TRequest, TResponse>  
    {
        if(!_behaviors.ContainsKey(typeof(TRequest)))
        {
            _behaviors[typeof(TRequest)] = new List<Type>();
        }

        var behaviorTypes = _behaviors[typeof(TRequest)];
        behaviorTypes.Add(typeof(TBehavior));
    }

    public void AddHandler<TRequest, TResponse, THandler>()
        where TRequest : IRequest<TResponse>
        where THandler : IHandler<TRequest, TResponse>
    {
        _handlers[typeof(IHandler<TRequest, TResponse>)] = typeof(THandler);
        _handlerRequests[typeof(IHandler<TRequest, TResponse>)] = typeof(TRequest);
        _handlerResponses[typeof(IHandler<TRequest, TResponse>)] = typeof(TResponse);
    }

    public IEnumerable<Type> GetBehaviors<TRequest,TResponse>() 
        where TRequest: IRequest<TResponse>
    {
        if (!_behaviors.ContainsKey(typeof(TRequest)))
        {
            return Array.Empty<Type>();
        }

        return _behaviors[typeof(TRequest)];
    }

    public IEnumerable<Type> GetBehaviors()
    {
        return _behaviors.Values.Where(x => x.Any()).SelectMany(x => x);
    }

    public IReadOnlyDictionary<Type,Type> GetHandlers()
    {
        return _handlers;
    }

    public IReadOnlyDictionary<Type, Type> HandlerRequests =>
        _handlerRequests;

    public IReadOnlyDictionary<Type, Type> HandlerResponses =>
        _handlerResponses;
}
