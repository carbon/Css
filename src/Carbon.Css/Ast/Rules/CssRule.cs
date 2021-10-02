namespace Carbon.Css;

public abstract class CssRule : CssBlock
{
    public CssRule()
        : base(NodeKind.Rule)
    { }

    public abstract RuleType Type { get; }
}
