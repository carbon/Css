using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Carbon.Css
{
	// Component Values 
	// Comma seperated list of a component values

	public class CssValueList : CssValue, IEnumerable<CssNode>
	{
		private readonly List<CssNode> children = new List<CssNode>();

		private readonly ValueSeperator seperator;

		public CssValueList(ValueSeperator seperator = ValueSeperator.Comma)
			: base(NodeKind.ValueList)
		{
			this.seperator = seperator;
		}

		public CssValueList(IEnumerable<CssNode> values, ValueSeperator seperator = ValueSeperator.Comma)
			: base(NodeKind.ValueList)
		{
			this.children.AddRange(values);

			this.seperator = seperator;
		}

		public ValueSeperator Seperator => seperator;

		public void Add(CssNode node)
		{
			node.Parent = this;

			children.Add(node);
		}

		public IList<CssNode> Children => children;

		public override CssNode CloneNode()
		{
			return new CssValueList(children.Select(c => c.CloneNode()), seperator);
		}

		public override string ToString()
		{
			return string.Join(seperator == ValueSeperator.Space ? " " : ", ", children.Select(t => t.ToString()));
		}

		#region IEnumerator

		IEnumerator<CssNode> IEnumerable<CssNode>.GetEnumerator()
		{
			return Children.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return Children.GetEnumerator();
		}

		#endregion
	}

	public enum ValueSeperator
	{
		Comma,
		Space
	}
}