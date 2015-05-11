namespace Carbon.Css
{
	public sealed class MediaRule : CssRule
	{
		private readonly string ruleText;

		public MediaRule(string ruleText)
			: base(RuleType.Media) {

			this.ruleText = ruleText;
		}

		public string RuleText => ruleText;
	}
}