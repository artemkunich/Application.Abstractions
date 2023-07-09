namespace Akunich.Application.Abstractions;

public delegate TRequest MapNotificationDelegate<in TNotification, out TRequest, TResponse>(TNotification notification) 
    where TNotification : INotification 
    where TRequest : IRequest<TResponse>;