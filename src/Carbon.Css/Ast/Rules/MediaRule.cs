using System.Linq;

namespace Carbon.Css;

public sealed class MediaRule(TokenList queryList) : CssRule
{
    public override RuleType Type => RuleType.Media;

    // QueryList?

    public TokenList Queries { get; } = queryList;

    public override CssNode CloneNode()
    {
        var rule = new MediaRule(Queries);

        rule.AddRange(Children);

        return rule;
    }
}