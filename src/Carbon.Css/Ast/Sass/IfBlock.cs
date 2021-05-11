namespace Carbon.Css
{
    public sealed class IfBlock : CssBlock
    {
        public IfBlock(CssValue condition)
            : base(NodeKind.If)
        {
            Condition = condition;
        }

        public CssValue Condition { get; }

        public override IfBlock CloneNode()
        {
            var block = new IfBlock(Condition);

            foreach (var child in children)
            {
                block.Add(child.CloneNode());
            }

            return block;
        }
    }
	
}