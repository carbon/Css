namespace Carbon.Css
{
    public sealed class PageRule : CssRule
    {
        public override RuleType Type => RuleType.Page;

        public PageRule(CssSelector? selector = null)
        {
            Selector = selector;
        }

        public CssSelector? Selector { get; }
    }
}