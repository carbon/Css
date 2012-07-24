namespace Carbon.Css
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;

	// A StyleRule rule has a selector, and one or more declarations

	public class CssRule
	{
		private readonly RuleType type;

		public CssRule(RuleType type)
		{
			this.type = type;
		}

		public RuleType Type
		{
			get { return type; }
		}

		public CssSelector Selector { get; set; }

		public CssBlock Block { get; set; }

		#region Helpers

		public void Expand()
		{
			new DefaultRuleTransformer().Transform(this);
		}

		#endregion

		public void WriteTo(TextWriter writer)
		{
			writer.Write(Selector.ToString() + " ");

			if (Block != null)
			{
				Block.WriteTo(writer);
			}
		}

		public override string ToString()
		{
			using (var writer = new StringWriter())
			{
				WriteTo(writer);

				return writer.ToString();
			}
		}
	}
}
