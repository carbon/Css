using System;

namespace Carbon.Css;

internal readonly struct CssScale : IEquatable<CssScale>
{
    public CssScale(CssValue x, CssValue y, CssValue z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    // e.g. 100px
    public CssValue X { get; }

    public CssValue Y { get; }

    public CssValue Z { get; }

    public bool Equals(CssScale other)
    {
        return X.Equals(other.X)
            && Y.Equals(other.Y)
            && Z.Equals(other.Z);
    }

    public override bool Equals(object? obj)
    {
        return obj is CssScale other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }
}