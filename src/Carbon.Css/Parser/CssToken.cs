namespace Carbon.Css.Parser
{
	public struct Token
	{
		private readonly TokenKind kind;
		private readonly string text;
		private readonly int position;

		public Token(TokenKind kind, char value, int position)
		{
			this.kind = kind;
			this.text = value.ToString();
			this.position = position;
		}

		public Token(TokenKind kind, string value, int position)
		{
			this.kind = kind;
			this.text = value;
			this.position = position;
		}

		public TokenKind Kind
		{
			get { return kind; }
		}

		public int Position
		{
			get { return position; }
		}

		public string Text
		{
			get { return text; }
		}


		public override string ToString()
		{
			return Kind + ": " + "'" + Text + "'";
		}

		#region Helpers

		public bool IsTrivia
		{
			get { return kind == TokenKind.Whitespace || kind == TokenKind.Comment; }
		}

		#endregion
	}

	public enum TokenKind
	{
		Whitespace,
		Comment,
		Identifier,		// selector or identifer (IDENT)
		
		Name,			// name (followed by a :)

		// Values
		String,
		Number,
		Percentage,		// {number}%
		Dimension,		// {number}px
		Variable,		// $name
		Uri,			// uri({string})


		AtKeyword,		// @{ident}
		Comma,			// ,
		Semicolon,		// ;
		Colon,			// :
		BlockStart,		// {
		BlockEnd,		// }
	}
}

