namespace Carbon.Css.Helpers;

public readonly struct LineInfo(int number, string text)
{
    public readonly int Number { get; } = number;

    public readonly string Text { get; } = text;
}