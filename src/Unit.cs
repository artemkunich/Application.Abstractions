namespace Akunich.Application.Abstractions;

public class Unit
{
    private static Unit _value;

    public static Unit Value => _value ??= new Unit();
}