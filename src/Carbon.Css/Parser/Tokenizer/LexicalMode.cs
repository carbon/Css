namespace Carbon.Css.Parser;

public enum LexicalMode
{
    Unknown            = 0,
    Rule               = 1,
    Block              = 2,
    Value              = 3,
    Declaration        = 4,
    Selector           = 5,
    Assignment         = 6,
    Function           = 7,
    Symbol             = 10,
    Unit               = 11,
    InterpolatedString = 13,
    Mixin              = 20
}