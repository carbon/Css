namespace Carbon.Css;

public sealed class StartingStyleRule : CssRule
{
    public override RuleType Type => RuleType.StartingStyle;

    public override StartingStyleRule CloneNode()
    {
        var clone = new StartingStyleRule() {
            Flags = Flags
        };

        foreach (var child in Children)
        {
            clone.Add(child.CloneNode());
        }

        return clone;
    }
}