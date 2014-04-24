namespace Carbon.Css
{
	using System.IO;

	public class MediaRule : CssRule
	{
		public MediaRule(string text)
			: base(RuleType.Media, new CssSelector(text)) { }
	}
}