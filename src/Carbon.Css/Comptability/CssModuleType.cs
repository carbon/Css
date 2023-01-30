namespace Carbon.Css;

public enum CssModuleType
{
    Animations            = 1,
    BackgroundsAndBorders = 2,  // http://www.w3.org/TR/css3-background/#background-position
    Core                  = 3,
    Color                 = 4,
    Columns               = 5,
    Containment           = 6, // https://www.w3.org/TR/css-contain-1/
    Flexbox               = 7, // https://www.w3.org/TR/css-flexbox-1/
    Fonts                 = 8, // http://dev.w3.org/csswg/css3-fonts/
    Masking               = 9, // http://dev.w3.org/fxtf/masking/
    Ruby                  = 10,
    UI                    = 11,
    Text                  = 12,
    Transitions           = 13,
    Transforms            = 14
}