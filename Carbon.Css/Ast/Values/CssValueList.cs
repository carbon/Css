namespace Carbon.Css
{
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	// Component Values 
	// Comma seperated list of a component values

	public class CssValueList : CssValue
	{
		private ValueListSeperator seperator;

		public CssValueList(ValueListSeperator seperator = ValueListSeperator.Comma)
			: base(NodeKind.ValueList)
		{
			this.seperator = seperator;
		}

		public CssValueList(IEnumerable<CssNode> values, ValueListSeperator seperator = ValueListSeperator.Comma)
			: base(NodeKind.ValueList)
		{
			this.children.AddRange(values);

			this.seperator = seperator;
		}

		public ValueListSeperator Seperator
		{
			get { return seperator; }
		}

		public override string Text
		{
			get { return ToString(); }
		}

		public override string ToString()
		{
			return string.Join(seperator == ValueListSeperator.Space ? " " : ", ", children.Select(t => t.ToString()));
		}
	}

	public enum ValueListSeperator
	{
		Comma,
		Space
	}

}
