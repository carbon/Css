namespace Carbon.Css.Parser
{
	using System;

	public class CssTokenizer : IDisposable
	{
		private readonly SourceReader reader;
		private readonly LexicalModeContext mode;

		private CssToken current;

		public CssTokenizer(SourceReader reader, LexicalMode mode = LexicalMode.Selector)
		{
			this.reader = reader;

			this.reader.Next(); // Start the reader

			this.mode = new LexicalModeContext(mode);

			this.Next(); // Load the first token
		}

		public CssToken Current
		{
			get { return current; }
		}

		public bool IsEnd
		{
			get { return isEnd; }
		}

		public CssToken Read(TokenKind expect, LexicalMode mode)
		{
			if (current.Kind != expect)
			{
				throw new ParseException(string.Format("Expected {0} reading {1}. Was {2}.", expect, mode.ToString(), this.current));
			}

			return Read();
		}

		private bool isEnd = false;

		// Returns the current token and advances to the next
		public CssToken Read()
		{
			if (isEnd) throw new Exception("Already ready the last token");

			var c = this.current;

			if (!reader.IsEof)	Next();
			else				isEnd = true;

			return c;
		}

		private CssToken Next()
		{
			this.current = ReadNext();

			return this.current;
		}

		private CssToken ReadNext()
		{
			if (reader.IsEof) throw new Exception("Cannot read past EOF. Current: " + current.ToString() + ".");

			switch (reader.Current)
			{
				case '\t': 
				case '\n': 
				case '\r':
 				case '\uFEFF':
				case ' ': return ReadWhitespace();

				case '@': return new CssToken(TokenKind.AtSymbol, reader.Read(), reader.Position);

				case '$': mode.Enter(LexicalMode.Symbol);					return new CssToken(TokenKind.Dollar, reader.Read(), reader.Position);

				case '/': return ReadComment();

				case ':': mode.Enter(LexicalMode.Value);					return new CssToken(TokenKind.Colon,		reader.Read(), reader.Position);
				case ',':													return new CssToken(TokenKind.Comma,		reader.Read(), reader.Position);
				case ';': LeaveValueMode();									return new CssToken(TokenKind.Semicolon,	reader.Read(), reader.Position);
				case '{':					mode.Enter(LexicalMode.Block);	return new CssToken(TokenKind.BlockStart,	reader.Read(), reader.Position);
				case '}': LeaveValueMode(); mode.Leave(LexicalMode.Block);	return new CssToken(TokenKind.BlockEnd,		reader.Read(), reader.Position);

				case '(': return new CssToken(TokenKind.LeftParenthesis,  reader.Read(), reader.Position);
				case ')': return new CssToken(TokenKind.RightParenthesis, reader.Read(), reader.Position);
				
				// case '-': // Conflicts with -webkit
				case '0':
				case '1':
				case '2':
				case '3':
				case '4':
				case '5':
				case '6':
				case '7':
				case '8':
				case '9':
					return ReadNumber();

			}

			switch (mode.Current)
			{
				case LexicalMode.Symbol		: return ReadSymbol();
				case LexicalMode.Value		: return ReadValue();
				default						: return ReadName();
			}
		}

		private CssToken ReadSymbol()
		{
			reader.Mark();

			while (!reader.IsWhiteSpace
				&& reader.Current != '{' && reader.Current != '}' && reader.Current != '(' && reader.Current != ')' 
				&& reader.Current != ';' && reader.Current != ':' && reader.Current != ',')
			{
				if (reader.IsEof) throw ParseException.UnexpectedEOF("Name");

				reader.Next();
			}

			mode.Leave(LexicalMode.Symbol);

			return new CssToken(TokenKind.Name, reader.Unmark(), reader.MarkStart);
		}

		private CssToken ReadName()
		{
			reader.Mark();

			while (!reader.IsWhiteSpace 
				&& reader.Current != '{' && reader.Current != '}' && reader.Current != '(' && reader.Current != ')' 
				&& reader.Current != ';' && reader.Current != ':' && reader.Current != ',')
			{
				if (reader.IsEof) throw ParseException.UnexpectedEOF("Name");

				reader.Next();
			}

			return new CssToken(TokenKind.Name, reader.Unmark(), reader.MarkStart);
		}

		private void LeaveValueMode()
		{
			if (mode.Current == LexicalMode.Value)
			{
				mode.Leave(LexicalMode.Value);
			}
		}

		private CssToken ReadValue()
		{
			reader.Mark();

			while (!reader.IsWhiteSpace 
				&& reader.Current != '}' && reader.Current != ')' && reader.Current != '(' && reader.Current != ';' && reader.Current != ',' && !reader.IsEof)
			{
				reader.Next();
			}

			return new CssToken(TokenKind.String, reader.Unmark(), reader.MarkStart);
		}

		private CssToken ReadNumber()
		{
			reader.Mark();

			while ((Char.IsDigit(reader.Current) || reader.Current == '.' || reader.Current == '-') && !reader.IsEof)
			{
				reader.Next();
			}

			return new CssToken(TokenKind.Number, reader.Unmark(), reader.MarkStart);
		}

		private CssToken ReadWhitespace()
		{
			reader.Mark();

			while (reader.IsWhiteSpace && !reader.IsEof)
			{
				reader.Next();
			}

			return new CssToken(TokenKind.Whitespace, reader.Unmark(), reader.MarkStart);
		}

		private CssToken ReadComment()
		{
			/* */

			reader.Mark();

			reader.Read();					// read /

			if (reader.Current == '/')
			{
				return ReadLineComment();
			}

			reader.Read();					// read *

			while (reader.Current != '*')
			{
				if (reader.IsEof) throw new ParseException("Unexpected EOF reading comment");

				reader.Next();
			}

			reader.Read(); // read *
			reader.Read(); // read /

			return new CssToken(TokenKind.Comment, reader.Unmark(), reader.MarkStart);
		}

		private CssToken ReadLineComment()
		{
			// line comment

			reader.Next(); // read /

			while (reader.Current != '\n' && reader.Current != '\r')
			{
				if (reader.IsEof) break;

				reader.Next();
			}

			return new CssToken(TokenKind.Comment, reader.Unmark(), reader.MarkStart);
		}

		public void Dispose()
		{
			reader.Dispose();
		}
	}
}