using System;
namespace Carbon.Css
{
	public class CssExpression : CssNode
	{
		public CssExpression()
			: base(NodeKind.Expression) { }

		public override string Text
		{
			get { return ""; }
		}

		public override CssNode Clone()
		{
			throw new NotImplementedException();
		}
	}
}