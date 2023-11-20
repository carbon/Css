using System.Collections.Frozen;

namespace Carbon.Css;

internal static class CssFunctionNames
{
    private static readonly FrozenSet<string> s_items = FrozenSet.ToFrozenSet([
        "abs",
        "attr",
        "asin",
        "acos",
        "atan",
        "atan2",
        "calc",
        "cross-fade",
        "cos",
        "cubic-bezier",
        "env",
        "exp",
        "hypot",
        "var",
        "round",
        "log",
        "max",
        "min",
        "mod",
        "pow",
        "rem",
        "sign",
        "sin",
        "sqrt",
        "tan"
    ]);

    public static bool Contains(string name) => s_items.Contains(name);
}