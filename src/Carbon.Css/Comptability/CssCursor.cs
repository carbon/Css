using System.Collections.Frozen;

namespace Carbon.Css;

// https://developer.mozilla.org/en-US/docs/Web/CSS/cursor

public static class CssCursor
{
    private static readonly CssCompatibility s_grabCursor = new(
        prefixed: new(firefox: 1.5f, safari: 4f),
        standard: new(chrome: 68, firefox: 27, edge: 15, safari: 11)
    );

    private static readonly CssCompatibility s_zoomCursor = new(
        prefixed: new(safari: 3),
        standard: new(chrome: 37, edge: 12, firefox: 24, safari: 9)
    );

    private static readonly FrozenDictionary<string, CssCompatibility> s_table = FrozenDictionary.Create<string, CssCompatibility>([
        new("auto",      CssCompatibility.All),
        new("alias",     CssCompatibility.All),
        new("copy",      CssCompatibility.All),
        new("crosshair", CssCompatibility.All),
        new("default",   CssCompatibility.All),
        new("e-resize",  CssCompatibility.All),
        new("grab",      s_grabCursor),
        new("grabbing",  s_grabCursor),
        new("help",      CssCompatibility.All),
        new("move",      CssCompatibility.All),
        new("n-resize",  CssCompatibility.All),
        new("ne-resize", CssCompatibility.All),
        new("nw-resize", CssCompatibility.All),
        new("pointer",   CssCompatibility.All),
        new("progress",  CssCompatibility.All),
        new("s-resize",  CssCompatibility.All),
        new("se-resize", CssCompatibility.All),
        new("sw-resize", CssCompatibility.All),
        new("text",      CssCompatibility.All),
        new("wait",      CssCompatibility.All),
        new("w-resize",  CssCompatibility.All),
        new("zoom-in",   s_zoomCursor),
        new("zoom-out",  s_zoomCursor)
    ]);
        
    public static bool NeedsPatch(string value, BrowserInfo browser)
    {
        if (s_table.TryGetValue(value, out CssCompatibility? c))
        {
            if (c.IsPrefixed(browser))
            {
                return true;
            }
        }

        return false;
    }
}