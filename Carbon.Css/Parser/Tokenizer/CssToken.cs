namespace Carbon.Css.Parser
{
	public struct CssToken
	{
		private readonly TokenKind kind;
		private readonly string text;
		private readonly int position;

		public CssToken(TokenKind kind, char value, int position)
		{
			this.kind = kind;
			this.text = value.ToString();
			this.position = position;
		}

		public CssToken(TokenKind kind, string value, int position)
		{
			this.kind = kind;
			this.text = value;
			this.position = position;
		}

		public TokenKind Kind => kind;

		public int Position => position;

		public string Text => text;

		public override string ToString() => $"{Kind}: '{Text}'";


		#region Helpers

		public bool IsTrivia => kind == TokenKind.Whitespace || kind == TokenKind.Comment;

		public bool IsBinaryOperator => (int)kind > 30 && (int)kind < 60;

		public bool IsEqualityOperator => kind == TokenKind.Equals || kind == TokenKind.NotEquals;

		public bool IsLogicalOperator => kind == TokenKind.And || kind == TokenKind.Or;


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

