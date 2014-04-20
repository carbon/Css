namespace Carbon.Css.Parser
{
	using System;

	public class ParseException : Exception
	{
		public ParseException(string message)
			: base(message) { }

		public static ParseException UnexpectedEOF(string context)
		{
			return new ParseException(string.Format("Unexpected EOF reading '{0}'.", context));
		}

		public static ParseException Unexpected(CssToken token, string context)
		{
			return new ParseException(string.Format("Unexpected '{0}' reading '{1}'.", token, context));
		}
	}
}
