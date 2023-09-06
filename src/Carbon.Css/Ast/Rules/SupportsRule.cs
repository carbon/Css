namespace Carbon.Css;

public sealed class SupportsRule(TokenList queryList) : CssRule
{
    public override RuleType Type => RuleType.Supports;

    // | font-format()
    // | font-tech()
    // | selector

    public TokenList Queries { get; } = queryList;
}