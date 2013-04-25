namespace Carbon.Css.Parser
{
	using System;
	using System.IO;

	public class CssTokenizer : IDisposable
	{
		private readonly SourceReader reader;
		private readonly LexicalModeContext mode = new LexicalModeContext(LexicalMode.Selector);

		private Token current = null;

		public CssTokenizer(SourceReader reader)
		{
			this.reader = reader;

			this.reader.Next(); // Start the reader
		}

		public Token Current
		{
			get { return current; }
		}

		public bool IsEnd
		{
			get { return reader.IsEof; }
		}

		public Token Read(TokenKind expect, string context)
		{
			var c = current;

			if (current.Kind != expect)
			{
				throw new ParseException(string.Format("Expected {0} reading {1}. Was {2}.", expect, context, this.current));
			}

			Next();

			return c;
		}

		public Token Read()
		{
			var c = this.current;

			Next();

			return c;
		}

		public Token Next()
		{
			this.current = ReadNext();

			return this.current;
		}

		private Token ReadNext()
		{
			if (reader.IsEof) return null;

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

				case ';':									return new Token(TokenKind.Semicolon,	reader.Read(), reader.Position);
				case '{': mode.Enter(LexicalMode.Block);	return new Token(TokenKind.BlockStart,	reader.Read(), reader.Position);
				case '}': mode.Leave(LexicalMode.Block);	return new Token(TokenKind.BlockEnd,	reader.Read(), reader.Position);
			}

			switch (mode.Current)
			{
				case LexicalMode.Block:			return ReadName();		// Declaration name
				case LexicalMode.Value:			return ReadValue();		// Declaration value
				case LexicalMode.Selector:		return ReadSelector();

				default: throw new Exception("Unexpected lexical mode:" + this.mode);
			}
		}

		private Token ReadName()
		{
			reader.Mark();

			while (reader.Current != ':' && reader.Current != '{')
			{
				if (reader.IsEof) throw ParseException.UnexpectedEOF("Name");

				reader.Next();
			}

			if (reader.Current == '{')
			{
				// We were actually reading a selector instead an @ rule block
				return new Token(TokenKind.Identifier, reader.Unmark(), reader.MarkStart);
			}

			mode.Enter(LexicalMode.Value);

			return new Token(TokenKind.Name, reader.Unmark(), reader.MarkStart);
		}

		private Token ReadValue()
		{
			if (reader.Current == ':')
			{
				return new Token(TokenKind.Colon, reader.Read(), reader.Position);
			}

			reader.Mark();

			while (reader.Current != ';' && reader.Current != '}')
			{
				if (reader.IsEof) throw new ParseException("Unexpected EOF reading value");

				reader.Next();
			}

			mode.Leave(LexicalMode.Value);

			return new Token(TokenKind.Value, reader.Unmark(), reader.MarkStart);
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

			if (mode.Current == LexicalMode.Value)
			{
				mode.Leave(LexicalMode.Value);
			}
			else
			{
				mode.Enter(LexicalMode.Value);
			}

			// Followed by a value

			return new Token(TokenKind.VariableName, reader.Unmark(), reader.MarkStart);
		}

		private Token ReadAtKeyword()
		{
			// @import
			// @font

			reader.Mark();

			while (reader.Current != ' ' && reader.Current != ';' && reader.Current != '{')
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

			while (
				reader.Current == ' ' ||
				reader.Current == '\t' ||
				reader.Current == '\n' ||
				reader.Current == '\r' ||
				reader.Current == '\uFEFF'
			)
			{
				reader.Next();

				if (reader.IsEof) break;
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

		private Token ReadSelector()
		{
			reader.Mark();
			
			// read until { - block start 
			while (reader.Current != '{')
			{
				if (reader.IsEof) throw ParseException.UnexpectedEOF("Selector");

				if (reader.Current == ';')
				{
					// We were reading a value @import url('styles.css');

					return new Token(TokenKind.Value, reader.Unmark(), reader.MarkStart);					
				}

				reader.Next();
			}

			return new Token(TokenKind.Identifier, reader.Unmark(), reader.MarkStart);
		}


		public void Dispose()
		{
			reader.Dispose();
		}
	}
}