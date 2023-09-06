namespace Carbon.Css;

public sealed class ForBlock(
    CssVariable variable,
    CssValue start,
    CssValue end,
    bool isInclusive = true) : CssBlock(NodeKind.For)
{
    public CssVariable Variable { get; } = variable;

    public CssValue Start { get; } = start;

    public CssValue End { get; } = end;

    // If through
    public bool IsInclusive { get; } = isInclusive;
}

// @for $var from <start> through <end> 
// @for $var from <start> to <end> 

// @for $i from 1 through 4