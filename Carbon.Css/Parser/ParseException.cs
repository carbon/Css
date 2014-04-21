namespace Carbon.Css.Parser
{
	using Carbon.Css.Helpers;
	using System;
	using System.Collections.Generic;

	public class ParseException : Exception
	{
		public ParseException(string message)
			: base(message) { }

		public static ParseException UnexpectedEOF(string context)
		{
			return new ParseException(string.Format("Unexpected EOF reading '{0}'.", context));
		}
	}

	public class UnexpectedTokenException : ParseException
	{
		private readonly CssToken token;

		public UnexpectedTokenException(LexicalMode mode, CssToken token)
			: base(String.Format("Unexpected token reading {0}. Was '{1}'.", mode, token.Kind))
		{
			this.token = token;
		}

		public UnexpectedTokenException(LexicalMode mode, TokenKind expectedKind, CssToken token)
			: base(String.Format("Unexpected token reading {0}. Expected '{1}'. Was '{2}'.", mode, expectedKind, token.Kind))
		{
			this.token = token;
		}

		public UnexpectedTokenException(CssToken token)
			: base(String.Format("Unexpected token. Was '{0}:{1}'.", token.Kind, token.Text))
		{
			this.token = token;
		}

		public CssToken Token
		{
			get { return token; }
		}

		public int Position
		{
			get { return token.Position; }
		}


		public SourceLocation Location { get; set; }

		public IList<LineInfo> Lines { get; set; }
	}
}
