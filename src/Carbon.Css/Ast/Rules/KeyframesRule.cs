using System;

namespace Carbon.Css
{
    public sealed class KeyframesRule : CssRule
    {
        public KeyframesRule(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public override RuleType Type => RuleType.Keyframes;

        public string Name { get; }

        // TODO: Keyframes

        public override CssNode CloneNode()
        {
            var rule = new KeyframesRule(Name);

            foreach (var x in Children)
            {
                rule.Add(x.CloneNode());
            }

            return rule;
        }
    }
}