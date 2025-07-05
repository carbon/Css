namespace Carbon.Css;

using Parser;

public sealed class CssUrl(CssToken name, CssValue value) 
    : CssFunction(name.Text, value)
{
}