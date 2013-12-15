namespace Carbon.Css
{
	using System.IO;

	public class StyleRule : CssRule
	{
		public StyleRule(CssSelector selector)
			: base(RuleType.Style, selector) { }

		public StyleRule(string selector)
			: base(RuleType.Style, new CssSelector(selector)) { }
	}
}