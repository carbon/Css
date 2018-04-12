using System;

namespace Carbon.Css
{
    public sealed class UnknownRule : CssRule
    {
        public UnknownRule(string name, string selectorText)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            SelectorText = selectorText;
        }

        public override RuleType Type => RuleType.Unknown;

        public string Name { get; }

        public string SelectorText { get; }
    }
}