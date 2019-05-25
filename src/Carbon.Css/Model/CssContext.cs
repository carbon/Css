using System.Collections.Generic;
using System.Linq;

namespace Carbon.Css
{
    public class CssContext
    {
        private CompatibilityTable compatibility = default;

        private BrowserInfo[]? browserSupport = null;
        private Dictionary<string, MixinNode>? mixins;

        public Dictionary<string, MixinNode> Mixins
        {
            get => mixins ??= new Dictionary<string, MixinNode>();
        }

        public BrowserInfo[]? BrowserSupport => browserSupport;

        internal bool TryGetBrowser(BrowserType type, out BrowserInfo browser)
        {
            if (browserSupport != null)
            {
                for (int i = 0; i < browserSupport.Length; i++)
                {
                    ref BrowserInfo b = ref browserSupport[i];

                    if (b.Type == type)
                    {
                        browser = b;

                        return true;
                    }
                }
            }

            browser = default;

            return false;
        }

        public CompatibilityTable Compatibility => compatibility;

        public void SetCompatibility(params BrowserInfo[] targets)
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
                    case BrowserType.Chrome  : chrome  = browser.Version; break;
                    case BrowserType.Firefox : firefox = browser.Version; break;
                    case BrowserType.IE      : ie      = browser.Version; break;
                    case BrowserType.Safari  : safari  = browser.Version; break;
                }
            }

            compatibility = new CompatibilityTable(chrome, firefox, ie, safari);
        }
    }
}
