namespace Carbon.Css.Parser
{
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

        public override string ToString() => $"{Kind}: '{Text}'";

        public void Deconstruct(out TokenKind kind, out string text)
        {
            kind = Kind;
            text = Text;
        }

		#region Helpers

		public bool IsTrivia => Kind == TokenKind.Whitespace || Kind == TokenKind.Comment;

		public bool IsBinaryOperator => (int)Kind > 30 && (int)Kind < 60;

		public bool IsEqualityOperator => Kind == TokenKind.Equals || Kind == TokenKind.NotEquals;

		public bool IsLogicalOperator => Kind == TokenKind.And || Kind == TokenKind.Or;
        
		#endregion
	}

	public enum TokenKind
	{
		// Identifier,		// selector or identifer (IDENT)
		
		Name,		// name (followed by a :)

		// Values
		String,
		Number,
		Unit,			// {em,ex,in,cm,mm,pt,pc,px

		Uri,			// uri({string})

		Dollar,			// ${variableName}

		AtSymbol,		// @{ident}
		Comma,			// ,
		Semicolon,		// ;
		Colon,			// :

		BlockStart,		// {
		BlockEnd,		// }

		LeftParenthesis,	// (
		RightParenthesis,	// )

		// Trivia
		Directive,		// //= *
		Whitespace,    
		Comment,

		// Binary Operators ------------------------

		// Logical
		And		  = 30, // && 
		Or		  = 31,	// ||

		// Equality
		Equals	  = 40,	// ==
		NotEquals = 41,	// !=

		// Relational
		Gt		  = 50,	// > 
		Gte		  = 51, // >=
		Lt		  = 52, // <
		Lte		  = 53, // <=

		// Math
		Divide	  = 60, // /
		Multiply  = 61, // *
		Add		  = 62,	// +
		Subtract  = 62,	// -
		Mod		  = 63  // %
	}
}

