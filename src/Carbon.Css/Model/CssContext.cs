using System.Linq;

namespace Carbon.Css;

public sealed class CssContext
{
    private CompatibilityTable compatibility = default;

    private BrowserInfo[]? browserSupport = null;
    private Dictionary<string, MixinNode>? _mixins;

    public Dictionary<string, MixinNode> Mixins
    {
        get => _mixins ??= new Dictionary<string, MixinNode>();
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
        if (browserSupport is not null) return;

        browserSupport = targets.OrderBy(t => t.Prefix.Text).ToArray();

        float chrome = 0;
        float firefox = 0;
        float edge = 0;
        float safari = 0;

        foreach (var browser in browserSupport)
        {
            switch (browser.Type)
            {
                case BrowserType.Chrome  : chrome  = browser.Version; break;
                case BrowserType.Firefox : firefox = browser.Version; break;
                case BrowserType.Edge    : edge    = browser.Version; break;
                case BrowserType.Safari  : safari  = browser.Version; break;
            }
        }

        compatibility = new CompatibilityTable(chrome, edge, firefox, safari);
    }
}