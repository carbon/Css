namespace Carbon.Css
{
    public sealed class CharsetRule : CssRule
    {
        public CharsetRule(string text)
        {
            this.Encoding = text;
        }

        public override RuleType Type => RuleType.Charset;
        
        public string Encoding { get; }
    }
}

// @charset "UTF-8";

// https://www.iana.org/assignments/character-sets/character-sets.xhtml