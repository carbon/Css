namespace Carbon.Css
{
	using System.Collections.Generic;
	using System.IO;
	using System.Text;

	public class StyleRule : CssRule
	{
		private readonly CssSelector selector;

		public StyleRule(CssSelector selector)
			: base(RuleType.Style) {
		
			this.selector = selector;
		}

		public StyleRule(string selectorText)
			: this(new CssSelector(selectorText)) { }

		public StyleRule(string selectorText, IList<CssNode> children)
			: this(new CssSelector(selectorText)) 
		{
				foreach (var child in children)
				{
					child.Parent = this;

					base.Children.Add(child);
				}
		}


		public CssSelector Selector
		{
			get { return selector; }
		}

		public override CssNode CloneNode()
		{
			var clone = new StyleRule(selector);

			foreach(var child in Children)
			{
				clone.Add(child.CloneNode());
			}

			return clone;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			using (var sw = new StringWriter(sb))
			{
				var writer = new CssWriter(sw);

				writer.WriteStyleRule(this, 0);

				return sb.ToString();
			}
		}
	}
}