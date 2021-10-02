namespace Carbon.Css;

using Parser;

public sealed class CssUrl : CssFunction
{
    public CssUrl(CssToken name, CssValue value)
        : base(name.Text, value)
    { }
}
