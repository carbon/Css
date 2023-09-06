namespace Carbon.Css;

public sealed class IfBlock(CssValue condition) : CssBlock(NodeKind.If)
{
    public CssValue Condition { get; } = condition;

    public override IfBlock CloneNode()
    {
        var block = new IfBlock(Condition);

        foreach (var child in _children)
        {
            block.Add(child.CloneNode());
        }

        return block;
    }
}