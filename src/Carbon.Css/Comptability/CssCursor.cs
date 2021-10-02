using System.Collections.Generic;

namespace Carbon.Css;

// https://developer.mozilla.org/en-US/docs/Web/CSS/cursor

public static class CssCursor
{
    private static readonly CssCompatibility grabCursor = new (
        prefixed: new (firefox: 1.5f, safari: 4f),
        standard: new (chrome: 68, firefox: 27, edge: 15, safari: 11)
    );

    private static readonly CssCompatibility zoomCursor = new (
        prefixed: new (safari: 3),
        standard: new (chrome: 37, edge: 12, firefox: 24, safari: 9)
    );

    private static readonly Dictionary<string, CssCompatibility> table = new ()
    {
        ["auto"]      = CssCompatibility.All,
        ["alias"]     = CssCompatibility.All,
        ["copy"]      = CssCompatibility.All,
        ["crosshair"] = CssCompatibility.All,

        ["default"]  = CssCompatibility.All,
        ["e-resize"] = CssCompatibility.All,
        ["grab"]     = grabCursor,
        ["grabbing"] = grabCursor,

        ["help"] = CssCompatibility.All,

        ["move"]      = CssCompatibility.All,
        ["n-resize"]  = CssCompatibility.All,
        ["ne-resize"] = CssCompatibility.All,
        ["nw-resize"] = CssCompatibility.All,

        ["pointer"]   = CssCompatibility.All,
        ["progress"]  = CssCompatibility.All,
        ["s-resize"]  = CssCompatibility.All,
        ["se-resize"] = CssCompatibility.All,
        ["sw-resize"] = CssCompatibility.All,

        ["text"]    = CssCompatibility.All,
        ["wait"]    = CssCompatibility.All,
        ["w-resize"] = CssCompatibility.All,

        ["zoom-in"]  = zoomCursor,
        ["zoom-out"] = zoomCursor
    };
        
    public static bool NeedsPatch(string value, BrowserInfo browser)
    {
        if (table.TryGetValue(value, out CssCompatibility? c))
        {
            if (c.IsPrefixed(browser))
            {
                return true;
            }
        }

        return false;
    }
}