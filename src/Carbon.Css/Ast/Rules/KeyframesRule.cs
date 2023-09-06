namespace Carbon.Css;

public sealed class KeyframesRule(string name) : CssRule
{
    public override RuleType Type => RuleType.Keyframes;

    public string Name { get; } = name;

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