namespace Carbon.Css;

public readonly struct CssPatch
{
    public CssPatch(string name, CssValue value)
    {
        Name = name;
        Value = value;
    }

    public readonly string Name { get; }

    public readonly CssValue Value { get; }
}