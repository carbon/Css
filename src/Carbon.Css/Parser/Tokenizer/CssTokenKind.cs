namespace Carbon.Css.Parser;

public enum CssTokenKind
{
    // Identifier,		// selector or identifer (IDENT)

    Name,           // name (followed by a :)

    // Values
    String,
    Number,
    Unit,           // {em,ex,in,cm,mm,pt,pc,px

    Uri,            // uri({string})

    Dollar,         // ${variableName}

    AtSymbol,       // @{ident}
    Comma,          // ,
    Semicolon,      // ;
    Colon,          // :
    Ampersand,      // &
    Tilde,          // ~

    BlockStart,         // {
    BlockEnd,           // }

    LeftParenthesis,    // (
    RightParenthesis,   // )

    InterpolatedStringStart, // #{
    InterpolatedStringEnd,   // }
                             // Trivia

    Directive,      // //= *
    Whitespace,
    Comment,

    // Binary Operators ------------------------

    // Logical
    And = 30,    // && 
    Or  = 31,    // ||

    // Equality
    Equals    = 40, // ==
    NotEquals = 41, // !=

    // Relational
    Gt  = 50, // > 
    Gte = 51, // >=
    Lt  = 52, // <
    Lte = 53, // <=

    // Math
    Divide   = 60, // /
    Multiply = 61, // *
    Add      = 62, // +
    Subtract = 63, // -
    Mod      = 64  // %
}