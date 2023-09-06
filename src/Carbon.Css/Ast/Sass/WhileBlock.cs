namespace Carbon.Css;

public sealed class WhileBlock(CssValue condition) 
    : CssBlock(NodeKind.While)
{
    public CssValue Condition { get; } = condition;
}