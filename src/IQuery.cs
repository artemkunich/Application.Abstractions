namespace Akunich.Application.Abstractions;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}