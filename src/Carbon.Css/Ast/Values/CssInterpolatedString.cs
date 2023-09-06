namespace Carbon.Css;

// #{$name}
public sealed class CssInterpolatedString(CssValue expression) 
    : CssValue(NodeKind.InterpolatedString)
{
    public CssValue Expression { get; } = expression;
}