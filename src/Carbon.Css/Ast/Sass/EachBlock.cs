namespace Carbon.Css;

public sealed class EachBlock(IReadOnlyList<CssVariable> variables, CssValue enumerable) : CssBlock(NodeKind.Each)
{
    public IReadOnlyList<CssVariable> Variables { get; } = variables;

    public CssValue Enumerable { get; } = enumerable;
}