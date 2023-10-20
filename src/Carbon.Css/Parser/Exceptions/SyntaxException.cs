using Carbon.Css.Helpers;

namespace Carbon.Css.Parser;

public class SyntaxException(string message, int position = 0) 
    : Exception(message)
{
    public int Position { get; } = position;

    public SourceLocation Location { get; set; }

    public IList<LineInfo>? Lines { get; set; }

    public static SyntaxException UnexpectedEOF(string context)
    {
        return new SyntaxException($"Unexpected EOF reading '{context}'.");
    }
}