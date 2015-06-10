namespace Carbon.Css.Parser
{
	using System;

	public class CssTokenizer : IDisposable
	{
		private readonly SourceReader reader;
		private readonly LexicalModeContext mode;

		private CssToken current;
		private bool isEnd = false;

		public CssTokenizer(SourceReader reader, LexicalMode mode = LexicalMode.Selector)
		{
			this.reader = reader;

			this.reader.Next(); // Start the reader

			this.mode = new LexicalModeContext(mode);

			this.Next(); // Load the first token
		}

		public CssToken Current => current;

		public bool IsEnd => isEnd;

		public CssToken Read(TokenKind expect, LexicalMode mode)
		{
			if (current.Kind != expect)
			{
				throw new UnexpectedTokenException(mode, expect, current);
			}

			return Read();
		}

		// Returns the current token and advances to the next
		public CssToken Read()
		{
			if (isEnd) throw new Exception("Already ready the last token");

			var c = current;

			if (!reader.IsEof)	Next();
			else				isEnd = true;

			return c;
		}

		private CssToken Next()
		{
			current = ReadNext();

			return current;
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

				case '$': mode.Enter(LexicalMode.Symbol);							return new CssToken(TokenKind.Dollar, reader.Read(), reader.Position);

				case '/':
					var peek = reader.Peek();

					if (peek == '/' || peek == '*')
						return ReadComment();
					else
						return new CssToken(TokenKind.Divide, reader.Read(), reader.Position);

				case ':': mode.Enter(LexicalMode.Value);							return new CssToken(TokenKind.Colon,		reader.Read(), reader.Position);
				case ',':															return new CssToken(TokenKind.Comma,		reader.Read(), reader.Position);
				case ';': LeaveValueMode();											return new CssToken(TokenKind.Semicolon,	reader.Read(), reader.Position);
				case '{':					mode.Enter(LexicalMode.Block);			return new CssToken(TokenKind.BlockStart,	reader.Read(), reader.Position);
				case '}': LeaveValueMode(); mode.Leave(LexicalMode.Block, this);	return new CssToken(TokenKind.BlockEnd,		reader.Read(), reader.Position);

				case '(': return new CssToken(TokenKind.LeftParenthesis,  reader.Read(), reader.Position);
				case ')': return new CssToken(TokenKind.RightParenthesis, reader.Read(), reader.Position);
				

				case '!': // !=
					if (reader.Peek() == '=') return new CssToken(TokenKind.Divide, reader.Read(2), reader.Position - 1);
					
					else break;
				case '=': // ==
					if (reader.Peek() == '=') return new CssToken(TokenKind.Divide, reader.Read(2), reader.Position - 1);

					else break;

				case '+': return new CssToken(TokenKind.Add, reader.Read(), reader.Position);
				case '*': return new CssToken(TokenKind.Multiply, reader.Read(), reader.Position);

				case '-':
					if (Char.IsDigit(reader.Peek()))
						return ReadNumber();
					else if (reader.Peek() == ' ')
						return new CssToken(TokenKind.Subtract, reader.Read(), reader.Position);
					else
						break;

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
				case LexicalMode.Symbol	: return ReadSymbol();
				case LexicalMode.Value	: return ReadValue();
				case LexicalMode.Unit	: return ReadUnit();
				default					: return ReadName();
			}
		}

		private CssToken ReadUnit()
		{
			reader.Mark();

			while (Char.IsLetter(reader.Current) || reader.Current == '%')
			{
				if (reader.IsEof) throw ParseException.UnexpectedEOF("Name");

				reader.Next();
			}

			mode.Leave(LexicalMode.Unit);

			return new CssToken(TokenKind.Unit, reader.Unmark(), reader.MarkStart);
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
				&& reader.Current != ';' && reader.Current != ',')
			{
				if (reader.IsEof) throw ParseException.UnexpectedEOF("Name");

				if (reader.Current == ':')
				{
					var peek = reader.Peek();

					if (!(peek  >= 'a' && peek <= 'z')) break;
				}

				reader.Next();
			}


			return new CssToken(TokenKind.Name, reader.Unmark(), reader.MarkStart);
		}

		private void LeaveValueMode()
		{
			if (mode.Current == LexicalMode.Value)
			{
				mode.Leave(LexicalMode.Value, this);
			}
		}

		private CssToken ReadValue()
		{
			reader.Mark();

			while (!reader.IsWhiteSpace && reader.Current != '{' && reader.Current != '}' 
				&& reader.Current != ')' && reader.Current != '(' && reader.Current != ';' && reader.Current != ',' && !reader.IsEof)
			{
				reader.Next();
			}

			return new CssToken(TokenKind.String, reader.Unmark(), reader.MarkStart);
		}

		private CssToken ReadNumber()
		{
			reader.Mark();

			// Read a leading '-'
			if(reader.Current == '-') reader.Next();

			while ((Char.IsDigit(reader.Current) || reader.Current == '.') && !reader.IsEof)
			{
				reader.Next();
			}


			if (reader.Current == '%' || Char.IsLetter(reader.Current))
			{
				mode.Enter(LexicalMode.Unit);
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

			bool isDirective = reader.Current == '=';

			while (reader.Current != '\n' && reader.Current != '\r')
			{
				if (reader.IsEof) break;

				reader.Next();
			}

			return new CssToken(isDirective ? TokenKind.Directive : TokenKind.Comment, reader.Unmark(), reader.MarkStart);
		}

		public void Dispose()
		{
			reader.Dispose();
		}
	}
}