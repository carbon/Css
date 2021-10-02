namespace Carbon.Css;

public sealed class ForBlock : CssBlock
{
    public ForBlock(CssVariable variable, CssValue start, CssValue end, bool isInclusive = true)
        : base(NodeKind.For)
    {
        Variable = variable;
        Start = start;
        End = end;
        IsInclusive = isInclusive;
    }

    public CssVariable Variable { get; }

    public CssValue Start { get; }

    public CssValue End { get; }

    // If through
    public bool IsInclusive { get; }
}

// @for $var from <start> through <end> 
// @for $var from <start> to <end> 

// @for $i from 1 through 4