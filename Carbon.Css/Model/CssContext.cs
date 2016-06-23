using System.Collections.Generic;
using System.Linq;

namespace Carbon.Css
{
    public class CssContext
    {
        private CompatibilityTable compatibility = new CompatibilityTable();

        private Browser[] browserSupport = null;

        public Dictionary<string, MixinNode> Mixins { get; } = new Dictionary<string, MixinNode>();

        public Browser[] BrowserSupport => browserSupport;

        internal Browser? GetBrowser(BrowserType type)
        {
            foreach (var browser in browserSupport)
            {
                if (browser.Type == type) return browser;
            }

            return null;
        }

        public CompatibilityTable Compatibility => compatibility;

        public void SetCompatibility(params Browser[] targets)
        {
            if (browserSupport != null) return;

            browserSupport = targets.OrderBy(t => t.Prefix.Text).ToArray();

            float chrome = 0;
            float firefox = 0;
            float ie = 0;
            float safari = 0;

            foreach (var browser in browserSupport)
            {
                switch (browser.Type)
                {
                    case BrowserType.Chrome  : chrome = browser.Version; break;
                    case BrowserType.Firefox : firefox = browser.Version; break;
                    case BrowserType.IE      : ie = browser.Version; break;
                    case BrowserType.Safari  : safari = browser.Version; break;
                }
            }

            compatibility = new CompatibilityTable(chrome, firefox, ie, safari);
        }
        
    }

    public enum CssFormatting
    {
        Original = 1,
        Pretty = 2,
        None = 3
    }
}
