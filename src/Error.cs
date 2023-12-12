using System;

namespace Application.Abstractions;

public abstract class Error : IEquatable<Error>
{
    public string Code => GetType().Name.Replace("Error", String.Empty);
    public string Message { get; set; }

    public override bool Equals(object other)
    {
        if(other is Error error)
            return Code == error.Code;

        return false;
    }
    
    public override int GetHashCode() => base.GetHashCode();

    public bool Equals(Error other)
    {
        if (other is null)
            return false;

        return Code == other.Code;
    }

    public static bool operator == (Error a, Error b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator != (Error a, Error b) => !(a == b);

}