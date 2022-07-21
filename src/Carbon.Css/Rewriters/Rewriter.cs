using System.Collections.Generic;

namespace Carbon.Css;

public sealed class RewriterCollection : List<ICssRewriter>
{
    public IEnumerable<CssRule> Rewrite(CssRule rule, int index = 0)
    {
        if (Count == 0)
        {
            yield return rule;

            yield break;
        }

        // TODO: Pass along in order

        // Chain

        foreach (var r in this[index].Rewrite(rule))
        {
            if (Count > index + 1)
            {
                foreach (var r2 in Rewrite(r, ++index))
                {
                    yield return r2;
                }
            }
            else
            {
                yield return r;
            }
        }

    }
}