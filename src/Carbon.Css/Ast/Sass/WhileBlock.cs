namespace Carbon.Css
{
    public sealed class WhileBlock : CssBlock
    {
        public WhileBlock(CssValue condition)
            : base(NodeKind.While)
        {

            Condition = condition;
        }

        public CssValue Condition { get; }
    }
}