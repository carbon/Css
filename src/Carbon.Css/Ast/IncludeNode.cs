namespace Carbon.Css;

public sealed class IncludeNode(
    string name,
    CssValue? args) : CssNode(NodeKind.Include)
{
    public string Name { get; } = name;

    public CssValue? Args { get; } = args;

    public override IncludeNode CloneNode() => new(Name, Args);
}

// @include box-emboss(0.8, 0.05);