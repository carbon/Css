using System.Collections.Frozen;

namespace Carbon.Css;

internal static class CssFunctionNames
{
    private static readonly FrozenSet<string> s_items = FrozenSet.ToFrozenSet([
        "attr",
        "calc",
        "cubic-bezier",
        "var",
        "round",
        "mod",
        "rem", 
        "sin",
        "cos",
        "tag",
        "asin",
        "acos",
        "atan",
        "atan2",
        "sqrt",
        "hypot",
        "log",
        "exp",
        "abs",
        "sign"
    ]);

    public static bool Contains(string name) => s_items.Contains(name);
}
