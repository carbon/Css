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

		public override CssNode CloneNode() => new BinaryExpression(left, op, right);
    }

	public enum Op
	{
		And			= 30, // && 
		Or			= 31, // ||

		Equals		= 32, // ==
		NotEquals	= 33, // !=
		Gt			= 34, // > 
		Gte			= 35, // >=
		Lt			= 36, // <
		Lte			= 37, // <=

		// Operators
		Divide		= 38, // /
		Multiply	= 39, // *
		Add			= 40, // +
		Subtract	= 41, // -
		Mod			= 42  // %
	}
}