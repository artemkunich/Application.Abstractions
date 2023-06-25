namespace Akunich.Application.Abstractions;

public sealed class Unit
{
    private static Unit _value;

    public static Unit Value => _value ??= new Unit();
}