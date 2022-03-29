using System.Globalization;

namespace Carbon.Css.Helpers;

public readonly struct SourceLocation
{
    public SourceLocation(int position, int line, int column)
    {
        Position = position;
        Line = line;
        Column = column;
    }

    // 0 based
    public int Position { get; }

    // 1 based
    public int Line { get; }

    // 1 based
    public int Column { get; }

    public readonly override string ToString() => string.Create(CultureInfo.InvariantCulture, $"({Line},{Column})");
}