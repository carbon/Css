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

        public override CssNode CloneNode()
        {
            var block = new IfBlock(Condition);

            foreach (var child in children)
            {
                block.Add(child.CloneNode());
            }

            return block;
        }
    }

    /*
	public class EachBlock : CssNode
	{
		public EachBlock(string variableName, CssValue list)
			: base(NodeKind.Each)
		{
			VariableName = variableName;
			List = list;
		}

		public string Variable { get; }

		public CssValue List { get; }
	}

	public class ForBlock : CssNode
	{
		public ForBlock()
			: base(NodeKind.For)
		{ }

		public CssValue Variable { get; }

		public CssVariable List { get; }
	}

	public class WhileBlock : CssNode
	{
		public WhileBlock()
			: base(NodeKind.While)
		{ }
	}
	*/
}

/*
@each $current-color in $colors-list {
    $i: index($colors-list, $current-color);
    .stuff-#{$i} { 
        color: $current-color;
    }
}
*/
