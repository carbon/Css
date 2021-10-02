using System;
using System.Collections.Generic;
using Carbon.Css.Helpers;

namespace Carbon.Css.Parser;

public class SyntaxException : Exception
{
    public SyntaxException(string message, int position = 0)
        : base(message)
    {

        Position = position;
    }

    public int Position { get; }

    public SourceLocation Location { get; set; }

    public IList<LineInfo>? Lines { get; set; }

    public static SyntaxException UnexpectedEOF(string context)
    {
        return new SyntaxException($"Unexpected EOF reading '{context}'.");
    }
}