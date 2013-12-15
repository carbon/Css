namespace Carbon.Css.Parser
{
	public class Token
	{
		private readonly TokenKind kind;
		private readonly string value;
		private readonly int position;

		public Token(TokenKind kind, char value, int position)
		{
			this.kind = kind;
			this.value = value.ToString();
			this.position = position;
		}

		public Token(TokenKind kind, string value, int position)
		{
			this.kind = kind;
			this.value = value;
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

		public string Value
		{
			get { return value; }
		}

		public override string ToString()
		{
			return Kind + ": " + Value;
		}
	}

	public enum TokenKind
	{
		Whitespace,
		Comment,
		Identifier,		// selector or identifer (IDENT)
		
		Name,			// name (followed by a :)
		Value,

		Variable,		// $name
		AtKeyword,		// @{ident}
		Comma,			// ,
		Semicolon,		// ;
		Colon,			// :
		BlockStart,		// {
		BlockEnd,		// }
	}
}
