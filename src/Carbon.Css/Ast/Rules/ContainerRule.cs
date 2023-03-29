namespace Carbon.Css;

public sealed class ContainerRule : CssRule
{
    public ContainerRule(TokenList queryList)
    {
        Queries = queryList;
    }

    public override RuleType Type => RuleType.Container;

    // QueryList?

    public TokenList Queries { get; }
}