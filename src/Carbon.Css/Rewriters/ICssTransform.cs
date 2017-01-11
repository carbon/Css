using System.Collections.Generic;

namespace Carbon.Css
{
    public interface ICssRewriter
    {
        IEnumerable<CssRule> Rewrite(CssRule rule);
    }
}