namespace Carbon.Css
{
    public sealed class MediaRule : CssRule
    {
        public MediaRule(TokenList queryList)
        {
            Queries = queryList;
        }

        public override RuleType Type => RuleType.Media;

        // QueryList?

        public TokenList Queries { get; }
    }
}