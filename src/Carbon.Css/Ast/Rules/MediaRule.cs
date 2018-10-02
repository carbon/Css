namespace Carbon.Css
{
    public sealed class MediaRule : CssRule
    {
        public MediaRule(TokenList ruleText)
        {
            Queries = ruleText;
        }

        public override RuleType Type => RuleType.Media;

        // QueryList?

        public TokenList Queries { get; }
    }
}