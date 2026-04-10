using System.Collections.Frozen;

namespace Carbon.Css;

public static class PseudoElementNames
{
    private static readonly FrozenSet<string> s_items = [
        "after",
        "backdrop",  // Safari 15.4
        "before",
        "cue",
        "cue-region",
        "file-selector-button",
        "first-letter",
        "first-line",
        "grammar-error",
        "highlight",
        "marker",
        "part",
        "placeholder",
        "scroll-marker",
        "scroll-marker-group",
        "scroll-button",
        "selection",
        "slotted",
        "spelling-error",
        "target-text",
        "view-transition",
        "view-transition-group"
    ];

    public static bool Contains(string name) => s_items.Contains(name);
}