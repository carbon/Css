namespace Carbon.Css.Selectors;

public enum CssSelectorType
{
    None = 0,
    Tag,            // AKA type
    Id,             // #x
    Class,          // .x
    Attribute,
    Universal,      // *
    PseudoClass,
    PseudoElement,
    HasScope,        // :has()
    NestingParent,   // &
}