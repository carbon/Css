namespace Carbon.Css
{
	using System.IO;
	using System.Text;

	public class StyleRule : CssRule
	{
		private readonly ICssSelector selector;

		public StyleRule(CssSelector selector)
			: base(RuleType.Style) {
		
			this.selector = selector;
		}

		public StyleRule(string selectorText)
			: this(new CssSelector(selectorText)) { }


		public ICssSelector Selector
		{
			get { return selector; }
		}


		public override CssNode Clone()
		{
			var clone = new StyleRule(selector.Text);

			foreach(var child in Children)
			{
				clone.Add(child.Clone());
			}

			return clone;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			using (var sw = new StringWriter(sb))
			{
				var writer = new CssWriter(sw, new CssContext());

				writer.WriteStyleRule(this, 0);

				return sb.ToString();
			}
		}
	}
}