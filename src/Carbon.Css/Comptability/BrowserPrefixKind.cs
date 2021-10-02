using System;

namespace Carbon.Css;

[Flags]
public enum BrowserPrefixKind
{
    Moz    = 1,
    Ms     = 2,
    O      = 4,
    Webkit = 8
}