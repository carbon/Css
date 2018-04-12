using System.Collections.Generic;

namespace Carbon.Css
{
    // https://developer.mozilla.org/en-US/docs/Web/CSS/cursor

    public static class CssCursor
    {
        private static Dictionary<string, CssCompatibility> table = new Dictionary<string, CssCompatibility> {
            ["auto"] = CssCompatibility.All,
            ["alias"] = CssCompatibility.All,
            ["copy"] = CssCompatibility.All,
            ["crosshair"] = CssCompatibility.All,

            ["default"]   = CssCompatibility.All,
            ["e-resize"]  = CssCompatibility.All,
            ["grab"]      = new CssCompatibility(prefixed: new CompatibilityTable(firefox: 1.5f, safari: 4f), standard: new CompatibilityTable(firefox: 27)),
            ["grabbing"]  = new CssCompatibility(prefixed: new CompatibilityTable(firefox: 1.5f, safari: 4f), standard: new CompatibilityTable(firefox: 27)),

            ["help"] = CssCompatibility.All,

            ["move"] = CssCompatibility.All,
            ["n-resize"] = CssCompatibility.All,
            ["ne-resize"] = CssCompatibility.All,
            ["nw-resize"] = CssCompatibility.All,

            ["pointer"] = CssCompatibility.All,
            ["progress"] = CssCompatibility.All,
            ["s-resize"] = CssCompatibility.All,
            ["se-resize"] = CssCompatibility.All,
            ["sw-resize"] = CssCompatibility.All,

            ["text"] = CssCompatibility.All,
            ["wait"] = CssCompatibility.All,
            ["w-resize"] = CssCompatibility.All,

            ["zoom-in"]     = new CssCompatibility(prefixed: new CompatibilityTable(safari: 3f), standard: new CompatibilityTable(safari: 9, firefox: 24)),
            ["zoom-out"]    = new CssCompatibility(prefixed: new CompatibilityTable(safari: 3f), standard: new CompatibilityTable(safari: 9, firefox: 24))
        };


        public static bool NeedsPatch(string value, BrowserInfo browser)
        {
            if (table.TryGetValue(value, out CssCompatibility c))
            {
                if (c.IsPrefixed(browser))
                {
                    return true;
                }
            }

            return false;
        }
    }
}