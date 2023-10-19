using System.Globalization;

namespace Carbon.Css.Helpers;

public readonly struct SourceLocation(int position, int line, int column)
{
    // 0 based
    public int Position { get; } = position;

    // 1 based
    public int Line { get; } = line;

    // 1 based
    public int Column { get; } = column;

    public readonly override string ToString() => string.Create(CultureInfo.InvariantCulture, $"({Line},{Column})");
}