using Akunich.Application.Abstractions;

namespace Application.Space;

public class SpaceNotification : INotification
{
    public int BehaviorsCount { get; init; }
    
    public string Value { get; init; }
}