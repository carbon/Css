namespace Carbon.Css;

public sealed class CssComment : CssNode
{
    public CssComment(string text)
       : this(text.AsMemory()) { }

    public CssComment(ReadOnlyMemory<char> text)
        : base(NodeKind.Comment)
    {
        Text = text;
    }

    public ReadOnlyMemory<char> Text { get; }
}