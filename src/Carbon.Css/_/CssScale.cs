namespace Carbon.Css;

internal readonly struct CssScale(CssValue x, CssValue y, CssValue z) : IEquatable<CssScale>
{
    // e.g. 100px
    public CssValue X { get; } = x;

    public CssValue Y { get; } = y;

    public CssValue Z { get; } = z;

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