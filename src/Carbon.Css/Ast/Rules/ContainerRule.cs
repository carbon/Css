namespace Carbon.Css;

public sealed class ContainerRule(TokenList queryList) : CssRule
{
    public override RuleType Type => RuleType.Container;

    // QueryList?

    public TokenList Queries { get; } = queryList;
}