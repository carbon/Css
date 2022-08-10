namespace Carbon.Css;

[Flags]
public enum CssBlockFlags
{
    HasIncludes    = 1 << 0,
    HasChildMedia  = 1 << 1,
    HasChildBlocks = 1 << 2,
    IsComplex      = 1 << 3
}