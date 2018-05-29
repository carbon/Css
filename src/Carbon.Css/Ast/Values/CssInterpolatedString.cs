namespace Carbon.Css
{
    // #{$name}
    public sealed class CssInterpolatedString : CssValue
    {
        public CssInterpolatedString(CssValue expression)
            : base(NodeKind.InterpolatedString)
        {
            Expression = expression;
        }

        public CssValue Expression { get; }
    }
}