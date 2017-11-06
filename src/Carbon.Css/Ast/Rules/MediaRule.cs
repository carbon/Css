namespace Carbon.Css
{
	public sealed class MediaRule : CssRule
	{
		public MediaRule(string ruleText)
        { 
			RuleText = ruleText;
		}

        public override RuleType Type => RuleType.Media;

        public string RuleText { get; }
	}
}