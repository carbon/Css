namespace Carbon.Css
{
    public sealed class UnknownRule : CssRule
    {
        public UnknownRule(string name, TokenList? selector)
        {
            Name = name;
            Text = selector;
        }

        public override RuleType Type => RuleType.Unknown;

        public string Name { get; }

        public TokenList? Text { get; }
    }
}