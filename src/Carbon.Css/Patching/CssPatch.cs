namespace Carbon.Css;

public readonly struct CssPatch(string name, CssValue value)
{
    public readonly string Name { get; } = name;

    public readonly CssValue Value { get; } = value;
}