namespace Carbon.Css;

public sealed class SupportsRule : CssRule
{
    public SupportsRule(TokenList queryList)
    {
        Queries = queryList;
    }

    public override RuleType Type => RuleType.Supports;

    // | font-format()
    // | font-tech()
    // | selector

    public TokenList Queries { get; }
}