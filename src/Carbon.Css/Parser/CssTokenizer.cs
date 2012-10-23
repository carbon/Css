namespace Carbon.Css.Parser
{
	using System;
	using System.IO;

	public class CssTokenizer
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

				case '/': return ReadComment();

				case ';':									reader.Next(); /* Read ; */ return new Token(TokenKind.Semicolon,	";");
				case '{': mode.Enter(LexicalMode.Block);	reader.Next(); /* Read { */ return new Token(TokenKind.BlockStart,	"{");
				case '}': mode.Leave(LexicalMode.Block);	reader.Next(); /* Read } */ return new Token(TokenKind.BlockEnd, "}");
			}

			switch (mode.Current)
			{
				case LexicalMode.Block:			return ReadName();		// Property declaration name
				case LexicalMode.Value:			return ReadValue();		// Property declaration value
				case LexicalMode.Selector:		return ReadSelector();

				default: throw new Exception("Unexpected lexical mode:" + this.mode);
			}
		}

		private Token ReadName()
		{
			reader.Mark();

			while (reader.Current != ':' && reader.Current != '{')
			{
				if (reader.IsEof) throw new ParseException("Unexpected EOF reading name");

				reader.Next();
			}

			if (reader.Current == '{')
			{
				// We were actually reading a selector instead an @ rule block
				return new Token(TokenKind.Identifier, reader.Unmark());
			}

			mode.Enter(LexicalMode.Value);

			return new Token(TokenKind.Name, reader.Unmark());
		}

		private Token ReadValue()
		{
			if(reader.Current == ':') 
			{
				reader.Next(); /* Read : */

				return new Token(TokenKind.Colon, ":");
			}

			reader.Mark();

			while (reader.Current != ';' && reader.Current != '}')
			{
				if (reader.IsEof) throw new ParseException("Unexpected EOF reading value");

				reader.Next();
			}

			mode.Leave(LexicalMode.Value);

			return new Token(TokenKind.Value, reader.Unmark());
		}

		private Token ReadAtKeyword()
		{
			reader.Mark();

			// { (Start declaration)
			while (reader.Current != ' ' && reader.Current != ';' && reader.Current != '{')
			{
				if (reader.IsEof) throw new ParseException("Unexpected EOF reading AtKeyword");

				reader.Next();
			}

			// Followed by a block, string, or ;

			return new Token(TokenKind.AtKeyword, reader.Unmark());
		}

		private Token ReadWhitespace()
		{
			reader.Mark();

			while (
				reader.Current == '\t' ||
				reader.Current == '\n' ||
				reader.Current == '\r' ||
				reader.Current == ' ' ||
				reader.Current == '\uFEFF'
			)
			{
				reader.Next();

				if (reader.IsEof) break;
			}

			return new Token(TokenKind.Whitespace, reader.Unmark());
		}

		private Token ReadComment()
		{
			/* */

			reader.Mark();

			reader.Next(); // Read /
			reader.Next(); // Read *

			while (reader.Current != '*')
			{
				if (reader.IsEof) throw new ParseException("Unexpected EOF reading comment");

				reader.Next();
			}

			reader.Next(); // Read *
			reader.Next(); // Read /

			// Console.WriteLine("Comment:" + reader.Unmark());

			return new Token(TokenKind.Comment, reader.Unmark());
		}


		private Token ReadSelector()
		{
			reader.Mark();
			
			// { (Declaration start)
			while (reader.Current != '{')
			{
				if (reader.IsEof) throw new ParseException("Unexpected EOF reading selector");

				reader.Next();
			}

			return new Token(TokenKind.Identifier, reader.Unmark());
		}
	}
}