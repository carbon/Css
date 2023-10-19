using System.Collections.Frozen;

namespace Carbon.Css;

public static class PseudoElementNames
{
    private static readonly FrozenSet<string> s_items = FrozenSet.ToFrozenSet([
        "after",
        "backdrop",
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
        "selection",
        "slotted",
        "spelling-error",
        "target-text",
        "view-transition",
        "view-transition-group"
    ]);

    public static bool Contains(string name) => s_items.Contains(name);
}