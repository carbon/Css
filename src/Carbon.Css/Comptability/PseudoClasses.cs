using System.Collections.Frozen;

namespace Carbon.Css;

public static class PseudoClassNames
{
    private static readonly FrozenSet<string> s_items = FrozenSet.ToFrozenSet([
        "active", "any", "any-link", "autofill",
        "blank", "buffering",
        "checked", "current",
        "default", "dir", "disabled", 
        "empty", "enabled",
        "first", "first-line", "first-child", "first-of-type", "fullscreen", "focus", "focus-visible", "focus-within",
        "has", "host", "hover",
        "indeterminate", "in-range", "invalid", "is",
        "lang", "last-child", "last-of-type", "left", "link", "local-link",
        "modal",
        "not", "nth-child", "nth-last-child", "nth-last-of-type",
        "only-child", "only-of-type", "optional", "out-of-range",
        "paused", "placeholder-shown", "playing", "popover-open",
        "read-only", "read-write", "required", "right", "root", 
        "scope", "seeking", "stalled",
        "target",
        "user-invalid", "user-valid",
        "valid", "visited", "volume-locked",
        "where"
    ]);

    public static bool Contains(string name) => s_items.Contains(name);
}