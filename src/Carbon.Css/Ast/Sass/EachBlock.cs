namespace Carbon.Css;

public sealed class EachBlock : CssBlock
{
    public EachBlock(CssVariable variable, CssValue enumerable)
        : base(NodeKind.Each)
    {
        Variable = variable;
        Enumerable = enumerable;
    }

    public CssVariable Variable { get; }

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
