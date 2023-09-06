namespace Carbon.Css;

public sealed class PageRule(CssSelector? selector = null) : CssRule
{
    public override RuleType Type => RuleType.Page;

    public CssSelector? Selector { get; } = selector;
}