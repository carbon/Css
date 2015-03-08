namespace Carbon.Css
{
	using System.Collections.Generic;
	using System.Linq;

	// Component Values 
	// Comma seperated list of a component values

	public class CssValueList : CssValue
	{
		private readonly List<CssNode> children = new List<CssNode>();

		private ValueSeperator seperator;

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

		public ValueSeperator Seperator
		{
			get { return seperator; }
		}

		public override string Text
		{
			get { return ToString(); }
		}

		public void Add(CssNode node)
		{
			node.Parent = this;

			children.Add(node);
		}

		public override IList<CssNode> Children
		{
			get { return children; }
		}

		public override CssNode CloneNode()
		{
			return new CssValueList(this.children.Select(c => c.CloneNode()), this.seperator);
		}

		public override string ToString()
		{
			return string.Join(seperator == ValueSeperator.Space ? " " : ", ", children.Select(t => t.ToString()));
		}
	}

	public enum ValueSeperator
	{
		Comma,
		Space
	}

}
