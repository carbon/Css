namespace Carbon.Css
{
	
	public class UnaryExpression : CssValue
	{
		private readonly CssNode operand;
		private readonly Op op;

		public UnaryExpression(Op op, CssNode operand)
			: base(NodeKind.Expression)
		{
			this.operand = operand;
			this.op = op;
		}

		public Op Operator => op;

		public CssNode Operand => operand;
	}

	public class BinaryExpression : CssValue
	{
		// ||, &&, ==, !=
		// +, -, *, /, %

		private readonly CssValue left;
		private readonly Op op;
		private readonly CssValue right;

		public BinaryExpression(CssValue left, Op op, CssValue right)
			: base(NodeKind.Expression)
		{
			this.left = left;
			this.op = op;
			this.right = right;
		}

		public CssValue Left => left;
		public CssValue Right => right;

		public Op Operator => op;
	}

	public enum Op
	{
		Add, // +
		Subtract, // - 
		Multipy, // *
		Divided, // 
		Mod,	// %

		And,
		Or,

		Equals,
		NotEquals,
		Gt,
		Gte,
		Lt,
		Lte
	}
}