namespace Akunich.Application.Abstractions;

public interface ICommand<TKey> : IRequest<Unit>
{
    TKey Id { get; set; }
}

public interface ICommand<TKey, out TResponse> : IRequest<TResponse>
{
    TKey Id { get; set; }
}