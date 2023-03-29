namespace Carbon.Css;

public sealed class EachBlock : CssBlock
{
    public EachBlock(IReadOnlyList<CssVariable> variables, CssValue enumerable)
        : base(NodeKind.Each)
    {
        Variables = variables;
        Enumerable = enumerable;
    }

    public IReadOnlyList<CssVariable> Variables { get; }

    public CssValue Enumerable { get; }
}

/*
@each $current-color in $colors-list {
    $i: index($colors-list, $current-color);
    .stuff-#{$i} { 
        color: $current-color;
    }
}
*/
