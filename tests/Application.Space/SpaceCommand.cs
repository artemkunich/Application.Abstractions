using Application.Symbol;

namespace Application.Space;

public sealed class SpaceCommand : SymbolCommand
{
    public override string Symbol => " ";

    public bool RaiseNotification { get; set; }

    public SpaceCommand(int behaviorsCount) : base(behaviorsCount)
    {
    }
}