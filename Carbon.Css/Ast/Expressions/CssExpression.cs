namespace Carbon.Css
{
	public class CssExpression : CssNode
	{
		public CssExpression()
			: base(NodeKind.Expression) { }

		// Binary			(||, &&, ==)
		// Unary

		// Left, Right, Operator

		// Operand (data)
		// Operation (operator)

		public enum Operation
		{
			Add		 , // +
			Subtract , // - 
			Multipy  , // *
			Divided  , // /
		}

		// +- * / % ^
	}
}