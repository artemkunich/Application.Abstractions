using Application.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace FluentMediator.Internal;

internal interface INotificationHandler<in TEvent> where TEvent : INotification
{
    Task<Result> HandleAsync(TEvent @event, CancellationToken cancellation);
}