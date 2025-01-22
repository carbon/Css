namespace Carbon.Css.Selectors;

public sealed class AttributeSelector : Selector
{
    public AttributeSelector()
        : base(CssSelectorType.Attribute) { }

    // todo: breakout attribute name, and value
}

/*
public enum AttributeMatchType
{
    Exact,
    Set,
    List,
    Hyphen,
    Contain, // E[foo*="bar"]
    Begin,   // E[foo^="bar"]
    End      // E[foo$="bar"]
}
*/