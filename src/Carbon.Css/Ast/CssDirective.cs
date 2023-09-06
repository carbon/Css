namespace Carbon.Css;

public sealed class CssDirective(string name, string? value) : CssNode(NodeKind.Directive)
{
    public string Name { get; } = name;

    public string? Value { get; } = value;
}

/*
EXAMPLES:
//= support IE 11+
//= support Safari 5.1+
//= support Chrome 20+
*/
