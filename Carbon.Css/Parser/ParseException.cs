using System;
using System.Collections.Generic;

namespace Carbon.Css.Parser
{
	using Helpers;

	public class ParseException : Exception
	{
		private readonly int position = 0;

		public ParseException(string message, int position = 0)
			: base(message) { 
		
			this.position = position;
		}

		public int Position => position;

		public SourceLocation Location { get; set; }

		public IList<LineInfo> Lines { get; set; }

		public static ParseException UnexpectedEOF(string context)
		{
			return new ParseException($"Unexpected EOF reading '{context}'.");
		}
	}

	public class UnexpectedModeChange : ParseException
	{
		// "Current mode is:" + current + ". Leaving " + mode + "."

		private int position;

		public UnexpectedModeChange(LexicalMode currentMode, LexicalMode leavingMode, int position)
			: base($"Unexpected mode change. Expected {currentMode}. Was {leavingMode}.")
		{
			this.position = position;
		}
	}

	public class UnbalancedBlock : ParseException
	{
		// "Current mode is:" + current + ". Leaving " + mode + "."

		public UnbalancedBlock(LexicalMode currentMode, CssToken startToken)
			: base("The block is unclosed, '}' expected", startToken.Position) { }
	}

	public class UnexpectedTokenException : ParseException
	{
		private readonly CssToken token;

		public UnexpectedTokenException(LexicalMode mode, CssToken token)
			: base($"Unexpected token reading {mode}. Was '{token.Kind}'.")
		{
			this.token = token;
		}

		public UnexpectedTokenException(LexicalMode mode, TokenKind expectedKind, CssToken token)
			: base($"Unexpected token reading {mode}. Expected '{expectedKind}'. Was '{token.Kind}'.", token.Position)
		{
			this.token = token;
		}

		public UnexpectedTokenException(CssToken token)
			: base($"Unexpected token. Was '{token.Kind}:{token.Text}'.", token.Position)
		{
			this.token = token;
		}

		public CssToken Token => token;
	}
}
