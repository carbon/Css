namespace Carbon.Css.Parser
{
	using System;

	public class CssTokenizer : IDisposable
	{
		private readonly SourceReader reader;
		private readonly LexicalModeContext mode;

		private Token current;

		public CssTokenizer(SourceReader reader, LexicalMode mode = LexicalMode.Selector)
		{
			this.reader = reader;

			this.reader.Next(); // Start the reader

			this.mode = new LexicalModeContext(mode);
		}

		public Token Current
		{
			get { return current; }
		}

		public bool IsEnd
		{
			get { return reader.IsEof; }
		}

		public Token Read(TokenKind expect, LexicalMode mode)
		{
			var c = current;

			if (current.Kind != expect)
			{
				throw new ParseException(string.Format("Expected {0} reading {1}. Was {2}.", expect, mode.ToString(), this.current));
			}

			if(!IsEnd) Next();

			return c;
		}

		public Token Read()
		{
			var c = this.current;

			if (!IsEnd) Next();

			return c;
		}

		public Token Next()
		{
			this.current = ReadNext();

			return this.current;
		}

		private Token ReadNext()
		{
			if (reader.IsEof) throw new Exception("Cannot read past EOF. Current: " + current.ToString() + ".");

			switch (reader.Current)
			{
				case '\t': 
				case '\n': 
				case '\r':
 				case '\uFEFF':
				case ' ': return ReadWhitespace();

				case '@': return ReadAtKeyword();

				case '$': return ReadVariableKeyword();

				case '/': return ReadComment();

				case ':': mode.Enter(LexicalMode.Value);					return new Token(TokenKind.Colon,		reader.Read(), reader.Position);
				case ',':													return new Token(TokenKind.Comma,		reader.Read(), reader.Position);
				case ';': LeaveValueMode();									return new Token(TokenKind.Semicolon,	reader.Read(), reader.Position);
				case '{':					mode.Enter(LexicalMode.Block);	return new Token(TokenKind.BlockStart,	reader.Read(), reader.Position);
				case '}': LeaveValueMode(); mode.Leave(LexicalMode.Block);	return new Token(TokenKind.BlockEnd, reader.Read(), reader.Position);
			}

			switch (mode.Current)
			{
				case LexicalMode.Value		: return ReadValue();
				case LexicalMode.Selector	: return ReadSelectorToken();
				default						: return ReadName();
			}
		}

		private Token ReadSelectorToken()
		{
			reader.Mark();

			while (!reader.IsWhiteSpace && reader.Current != '{' && reader.Current != '}' && reader.Current != ';')
			{
				if (reader.IsEof) throw ParseException.UnexpectedEOF("Selector");

				reader.Next();
			}

			return new Token(TokenKind.Name, reader.Unmark(), reader.MarkStart);
		}


		private Token ReadName()
		{
			reader.Mark();

			while (!reader.IsWhiteSpace && reader.Current != '{' && reader.Current != '}' && reader.Current != ';' && reader.Current != ':')
			{
				if (reader.IsEof) throw ParseException.UnexpectedEOF("Name");

				reader.Next();
			}

			if (mode.Current == LexicalMode.Block)
			{
				return new Token(TokenKind.Identifier, reader.Unmark(), reader.MarkStart);
			}
			else
			{
				return new Token(TokenKind.Name, reader.Unmark(), reader.MarkStart);
			}
		}

		private void LeaveValueMode()
		{
			if (mode.Current == LexicalMode.Value)
			{
				mode.Leave(LexicalMode.Value);
			}
		}

		private Token ReadValue()
		{
			reader.Mark();

			while (!reader.IsWhiteSpace && reader.Current != '}' && reader.Current != ';' && !reader.IsEof)
			{
				reader.Next();
			}

			return new Token(TokenKind.String, reader.Unmark(), reader.MarkStart);
		}

		private Token ReadVariableKeyword()
		{
			// $blue

			reader.Mark();

			reader.Next(); // read $

			while (Char.IsLetter(reader.Current))
			{
				if (reader.IsEof) throw ParseException.UnexpectedEOF("Variable");

				reader.Next();
			}

			// Followed by a value

			return new Token(TokenKind.Variable, reader.Unmark(), reader.MarkStart);
		}

		private Token ReadAtKeyword()
		{
			// @import
			// @font

			reader.Mark();

			while (!reader.IsWhiteSpace && reader.Current != ';' && reader.Current != '{' && reader.Current != ':')
			{
				if (reader.IsEof) throw new ParseException("Unexpected EOF reading AtKeyword");

				reader.Next();
			}

			// Followed by a block ({), string, or ;

			return new Token(TokenKind.AtKeyword, reader.Unmark(), reader.MarkStart);
		}

		private Token ReadWhitespace()
		{
			reader.Mark();

			while (reader.IsWhiteSpace && !reader.IsEof)
			{
				reader.Next();
			}

			return new Token(TokenKind.Whitespace, reader.Unmark(), reader.MarkStart);
		}

		private Token ReadComment()
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

			return new Token(TokenKind.Comment, reader.Unmark(), reader.MarkStart);
		}

		private Token ReadLineComment()
		{
			// line comment

			reader.Next(); // read /

			while (reader.Current != '\n' && reader.Current != '\r')
			{
				if (reader.IsEof) break;

				reader.Next();
			}

			return new Token(TokenKind.Comment, reader.Unmark(), reader.MarkStart);
		}

		public void Dispose()
		{
			reader.Dispose();
		}
	}
}