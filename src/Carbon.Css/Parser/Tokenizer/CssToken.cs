namespace Carbon.Css.Parser;

public readonly struct CssToken
{
    public CssToken(TokenKind kind, char value, int position)
    {
        Kind = kind;
        Text = value.ToString();
        Position = position;
    }

    public CssToken(TokenKind kind, string value, int position)
    {
        Kind = kind;
        Text = value;
        Position = position;
    }

    public readonly TokenKind Kind;

    public readonly int Position;

    public readonly string Text;

    public readonly override string ToString() => $"{Kind}: '{Text}'";

    public readonly void Deconstruct(out TokenKind kind, out string text)
    {
        kind = Kind;
        text = Text;
    }

    #region Helpers

    public readonly bool IsTrivia => Kind is TokenKind.Whitespace or TokenKind.Comment;

    public readonly bool IsBinaryOperator => (int)Kind > 30 && (int)Kind < 65;

    public readonly bool IsEqualityOperator => Kind is TokenKind.Equals or TokenKind.NotEquals;

    public readonly bool IsLogicalOperator => Kind is TokenKind.And or TokenKind.Or;

    #endregion
}

public enum TokenKind
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
    And = 30, // && 
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