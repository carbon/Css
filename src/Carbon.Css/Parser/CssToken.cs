namespace Carbon.Css.Parser
{
	public class Token
	{
		private readonly TokenKind kind;
		private readonly string value;

		public Token(TokenKind kind, string value)
		{
			this.kind = kind;
			this.value = value;
		}

		public TokenKind Kind
		{
			get { return kind; }
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
		
		// Declaration,	// property: value

		/*
		DeclarationName,			// [property]:
		DeclarationValue,			// :[value]
		*/

		Name, 
		Value,

		// Identifier, // value

		Semicolon,  // ;
		Colon,		// :
		BlockStart,	// {
		BlockEnd,	// }

		AtKeyword	// @{ident}
	}
}
