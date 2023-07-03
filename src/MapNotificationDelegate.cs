namespace Akunich.Application.Abstractions;

public delegate TCommand MapNotificationDelegate<in TNotification, out TCommand>(TNotification notification) 
    where TNotification : INotification 
    where TCommand : IRequest<Unit>;