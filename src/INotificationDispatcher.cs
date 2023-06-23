using System.Threading;
using System.Threading.Tasks;

namespace Akunich.Application.Abstractions;

public interface INotificationDispatcher
{
    Task<Result<Unit>> DispatchAsync<TNotification>(TNotification request, CancellationToken cancellation = default) where TNotification: INotification;
}