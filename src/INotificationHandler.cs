using System.Threading;
using System.Threading.Tasks;

namespace Akunich.Application.Abstractions;

public interface INotificationHandler<in TEvent> where TEvent : INotification
{
    Task<Result<Unit>> HandleAsync(TEvent @event, CancellationToken cancellation);
}