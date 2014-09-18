namespace Carbon.Css
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;

	// A StyleRule rule has a selector, and one or more declarations

	public class CssRule : CssBlock
	{
		private readonly RuleType type;
		
		public CssRule(RuleType type)
			: base(NodeKind.Rule)
		{
			this.type = type;
		}

		public CssRule(RuleType type, NodeKind kind)
			: base(kind) 
		{	
			this.type = type;
		}

		public RuleType Type
		{
			get { return type; }
		}

		public override string Text
		{
			get { return ToString(); }
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			using (var sw = new StringWriter(sb))
			{
				new CssWriter(sw);

				return sb.ToString();
			}
		}
	}
}