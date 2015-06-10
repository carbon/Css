namespace Carbon.Css
{
	public class AtRule : CssRule
	{
		private readonly string name;
		private readonly string selectorText;

		public AtRule(string name, RuleType type, string selectorText)
			: base(type) {
		
			this.name = name;
			this.selectorText = selectorText;
		}

		public string Name => name;

		public string SelectorText => selectorText;
	}
}