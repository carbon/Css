namespace Carbon.Css;

public sealed class UnknownRule(string name, TokenList? selector) : CssRule
{
    public override RuleType Type => RuleType.Unknown;

    public string Name { get; } = name;

    public TokenList? Text { get; } = selector;
}
