namespace Carbon.Css;

public readonly struct CssParameter(
    string name,
    CssValue? defaultValue = null)
{
    public string Name { get; } = name;

    public CssValue? DefaultValue { get; } = defaultValue;
}