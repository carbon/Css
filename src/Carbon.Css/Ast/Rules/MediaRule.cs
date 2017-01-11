namespace Carbon.Css
{
	public class MediaRule : CssRule
	{
		public MediaRule(string ruleText)
			: base(RuleType.Media) {

			RuleText = ruleText;
		}

		public string RuleText { get; }
	}
}