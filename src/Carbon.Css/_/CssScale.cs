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
    public readonly CssValue X { get; }

    public readonly CssValue Y { get; }

    public readonly CssValue Z { get; }

    public bool Equals(CssScale other)
    {
        return X.Equals(other.X)
            && Y.Equals(other.Y)
            && Z.Equals(other.Z);
    }
}