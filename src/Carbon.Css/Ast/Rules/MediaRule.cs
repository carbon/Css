namespace Carbon.Css
{
	public sealed class MediaRule : CssRule
	{
		public MediaRule(TokenList ruleText)
        { 
			Text = ruleText;
		}

        public override RuleType Type => RuleType.Media;

        public TokenList Text { get; }
	}
}