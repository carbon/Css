namespace Carbon.Css
{ 
	public class UnaryExpression : CssValue
	{
		public UnaryExpression(Op op, CssNode operand)
			: base(NodeKind.Expression)
		{
			Operand = operand;
			Operator = op;
		}

		public Op Operator { get; }

		public CssNode Operand { get; }
	}

	public class BinaryExpression : CssValue
	{
		// ||, &&, ==, !=
		// +, -, *, /, %

		public BinaryExpression(CssValue left, Op op, CssValue right)
			: base(NodeKind.Expression)
		{
			Left = left;
			Operator = op;
			Right = right;
		}

		public CssValue Left { get; }
		public CssValue Right { get; }
		public Op Operator { get; }

		public override CssNode CloneNode() => new BinaryExpression(Left, Operator, Right);
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