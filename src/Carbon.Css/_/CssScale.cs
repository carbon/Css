namespace Carbon.Css
{
    internal readonly struct CssScale
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
    }
}