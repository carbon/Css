namespace Carbon.Css
{
    public class KeyframesRule : CssRule
    {
        public KeyframesRule(string name)
            : base(RuleType.Keyframes)
        {

            Name = name;
        }

        public string Name { get; }

        // TODO: Keyframes

        public override CssNode CloneNode()
        {
            var rule = new KeyframesRule(Name);

            foreach (var x in children)
            {
                rule.Add(x.CloneNode());
            }

            return rule;
        }
    }
}