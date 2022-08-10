namespace Carbon.Css.Parser;

public readonly struct CssToken: ISpanFormattable
{
    public CssToken(CssTokenKind kind, char value, int position)
    {
        Kind = kind;
        Text = value.ToString();
        Position = position;
    }

    public CssToken(CssTokenKind kind, string value, int position)
    {
        Kind = kind;
        Text = value;
        Position = position;
    }

    public CssTokenKind Kind { get; }

    public int Position { get; }

    public string Text { get; }

    public readonly override string ToString() => $"{Kind}: '{Text}'";

    public readonly void Deconstruct(out CssTokenKind kind, out string text)
    {
        kind = Kind;
        text = Text;
    }

    bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        return destination.TryWrite($"{Kind}: '{Text}'", out charsWritten);
    }

    string IFormattable.ToString(string? format, IFormatProvider? formatProvider)
    {
        return $"{Kind}: '{Text}'";
    }

    #region Helpers

    public readonly bool IsTrivia => Kind is CssTokenKind.Whitespace or CssTokenKind.Comment;

    public readonly bool IsBinaryOperator => (int)Kind > 30 && (int)Kind < 65;

    public readonly bool IsEqualityOperator => Kind is CssTokenKind.Equals or CssTokenKind.NotEquals;

    public readonly bool IsLogicalOperator => Kind is CssTokenKind.And or CssTokenKind.Or;

    #endregion
}