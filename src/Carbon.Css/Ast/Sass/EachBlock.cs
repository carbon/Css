namespace Carbon.Css;

public sealed class EachBlock : CssBlock
{
    public EachBlock(IReadOnlyList<CssVariable> variables, CssValue enumerable)
        : base(NodeKind.Each)
    {
        Variables = variables;
        Enumerable = enumerable;
    }

    public IReadOnlyList<CssVariable> Variables { get; }

    public CssValue Enumerable { get; }
}