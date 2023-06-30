using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Akunich.Application.Abstractions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, Assembly assembly) => services
        .AddRequestHandlers(assembly)
        .AddNotificationHandlers(assembly)
        .AddPipelineBehaviors(assembly)
        .AddPipelines(assembly);

    public static IServiceCollection AddRequestHandlers(this IServiceCollection services, Assembly assembly)
    {
        var handlerTypes = assembly.GetTypes().Where(t =>
            !t.IsAbstract && !t.IsInterface && !t.IsGenericType &&
            t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))
        ).ToList();

        foreach (var handlerType in handlerTypes)
        {
            var genericHandlerType = handlerType
                .GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>));
            if(genericHandlerType is null)
                continue;
            
            services.AddScoped(genericHandlerType, handlerType);
        }
        
        return services;
    }
    
    public static IServiceCollection AddNotificationHandlers(this IServiceCollection services, Assembly assembly)
    {
        var handlerTypes = assembly.GetTypes().Where(t =>
            !t.IsAbstract && !t.IsInterface && !t.IsGenericType &&
            t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(INotificationHandler<>))
        ).ToList();

        foreach (var handlerType in handlerTypes)
        {
            var genericHandlerType = handlerType
                .GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(INotificationHandler<>));
            if(genericHandlerType is null)
                continue;
            
            services.AddScoped(genericHandlerType, handlerType);
        }

        return services;
    }

    public static IServiceCollection AddPipelineBehaviors(this IServiceCollection services, Assembly assembly)
    {
        var behaviorTypes = assembly.GetTypes().Where(t =>
            !t.IsAbstract && !t.IsInterface &&
            t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPipelineBehavior<,>))
        ).ToList();

        foreach (var behaviorType in behaviorTypes)
        {
            var genericBehaviorType = behaviorType
                .GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPipelineBehavior<,>));
            if(genericBehaviorType is null)
                continue;
            
            if(behaviorType.IsGenericType)
                services.AddScoped(typeof(IPipelineBehavior<,>), behaviorType.GetGenericTypeDefinition());
            else
                services.AddScoped(genericBehaviorType, behaviorType);
        }
        
        return services;
    }
    
    public static IServiceCollection AddPipelines(this IServiceCollection services, Assembly assembly)
    {
        var piplineTypes = assembly.GetTypes().Where(t =>
            !t.IsAbstract && !t.IsInterface &&
            t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPipeline<,>))
        ).ToList();

        foreach (var pipelineType in piplineTypes)
        {
            var genericPipelineType = pipelineType
                .GetInterfaces().FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPipeline<,>));
            if(genericPipelineType is null)
                continue;
            
            if(pipelineType.IsGenericType)
                services.AddScoped(typeof(IPipeline<,>), pipelineType.GetGenericTypeDefinition());
            else
                services.AddScoped(genericPipelineType, pipelineType);
        }
        
        return services;
    }
}