namespace Carbon.Css
{
	public class AtRule : CssRule
	{
		public AtRule(string name, RuleType type, string selectorText)
			: base(type) {
		
			Name = name;
			SelectorText = selectorText;
		}

		public string Name { get; }

		public string SelectorText { get; }
    }
}