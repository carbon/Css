namespace Carbon.Css;

public static class PseudoClassNames
{
    private static readonly HashSet<string> items = [
        "active", "after", "any", "any-link", "before", "checked", "cue", "default", "dir", "disabled", "empty", "enabled",
        "first", "first-letter", "first-line", "first-child", "first-of-type", "fullscreen", "focus", "focus-within",
        "hover", "indeterminate", "in-range", "invalid", "lang", "last-child", "last-of-type", "left", "link",
        "not", "nth-child", "nth-last-child", "nth-last-of-type", "only-child", "only-of-type", "optional",
        "out-of-range",
        "placeholder-shown", "read-only", "read-write", "required", "right", "root", "scope",
        "target", "valid", "visited"
    ];


    public static bool Contains(string name) => items.Contains(name);
}