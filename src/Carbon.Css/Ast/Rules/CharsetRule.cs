namespace Carbon.Css
{
    public sealed class CharsetRule : CssRule
    {
        public CharsetRule(string text)
        {
            this.Text = text;
        }

        public override RuleType Type => RuleType.Charset;
        
        public string Text { get; }
    }

    // @charset "UTF-8";
}