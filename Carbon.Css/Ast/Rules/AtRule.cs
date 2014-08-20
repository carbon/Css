namespace Carbon.Css
{
	public class AtRule : CssRule
	{
		private readonly string atName;
		private readonly string selectorText;

		public AtRule(string name, RuleType type, string selectorText)
			: base(type) {
		
			this.atName = name;
			this.selectorText = selectorText;
		}

		public string AtName
		{
			get { return atName; }
		}

		public string SelectorText
		{
			get { return selectorText; }
		}
	}
}