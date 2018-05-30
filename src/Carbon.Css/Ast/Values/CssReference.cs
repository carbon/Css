namespace Carbon.Css
{
    using Parser;

    public sealed class CssReference : CssValue
    {
        public CssReference(string name)
            : base(NodeKind.Reference)
        {
            Name = name;
        }

        public CssReference(CssToken name)
            : base(NodeKind.Reference)
        {
            Name = name.Text;
        }

        public string Name { get; }

        public CssSequence Value { get; set; }
    }
}