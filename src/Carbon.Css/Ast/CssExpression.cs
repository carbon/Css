using System.Collections.Generic;
namespace Carbon.Css.Ast
{
	public class CssExpression
	{
		private readonly IList<CssNode> nodes = new List<CssNode>();

		public CssExpression() { }

		public CssNode Children { get; set; }
	}
}