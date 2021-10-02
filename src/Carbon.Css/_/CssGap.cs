#pragma warning disable IDE0057 // Use range operator

using System;
using System.Runtime.Serialization;

namespace Carbon.Css;

public readonly struct CssGap : IEquatable<CssGap>
{
    public CssGap(double value)
        : this(new CssUnitValue(value, CssUnitInfo.Px)) { }

    public CssGap(CssUnitValue value)
    {
        X = value;
        Y = value;
    }

    public CssGap(CssUnitValue x, CssUnitValue y)
    {
        X = x;
        Y = y;
    }

    [DataMember(Name = "x", Order = 1)]
    public readonly CssUnitValue X { get; }

    [DataMember(Name = "y", Order = 2)]
    public readonly CssUnitValue Y { get; }

    // 1px 1px
    // 1px

    public readonly override string ToString()
    {
        if (X.Equals(Y))
        {
            return X.ToString();
        }
      

        return $"{X} {Y}";
    }

    public static CssGap Parse(string text)
    {
        return Parse(text.AsSpan());
    }

    // 1 or 2 values
    public static CssGap Parse(ReadOnlySpan<char> text)
    {
        int spaceIndex = text.IndexOf(' ');

        if (spaceIndex > -1)
        {
            var x = text.Slice(0, spaceIndex);
            var y = text.Slice(spaceIndex + 1);

            return new CssGap(CssUnitValue.Parse(x), CssUnitValue.Parse(y));
        }

        return new CssGap(CssUnitValue.Parse(text));
    }

    public bool Equals(CssGap other)
    {
        return X.Equals(other.X)
            && Y.Equals(other.Y);
    }

    public override bool Equals(object? obj)
    {
        return obj is CssGap other && Equals(other);
    }

    public override int GetHashCode() => HashCode.Combine(X, Y);

    public static bool operator ==(CssGap left, CssGap right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(CssGap left, CssGap right)
    {
        return !left.Equals(right);
    }
}
