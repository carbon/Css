namespace Carbon.Css;

public sealed class KeyframesRule : CssRule
{
    public KeyframesRule(string name)
    {
        Name = name;
    }

    public override RuleType Type => RuleType.Keyframes;

    public string Name { get; }

    // TODO: Keyframes

    public override CssNode CloneNode()
    {
        var rule = new KeyframesRule(Name);

        foreach (var child in Children)
        {
            rule.Add(child.CloneNode());
        }

        return rule;
    }
}