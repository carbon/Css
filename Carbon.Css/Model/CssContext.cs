using System.Collections.Generic;
using System.Linq;

namespace Carbon.Css
{
    public class CssContext
    {
        private Browser[] browserSupport = null;

        public Dictionary<string, MixinNode> Mixins { get; } = new Dictionary<string, MixinNode>();

        public Browser[] BrowserSupport => browserSupport;

        public void SetCompatibility(params Browser[] targets)
        {
            if (browserSupport != null) return;

            browserSupport = targets.OrderBy(t => t.Prefix.Text).ToArray();
        }
    }

    public enum CssFormatting
    {
        Original = 1,
        Pretty = 2,
        None = 3
    }
}
