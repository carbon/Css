namespace Carbon.Css
{
    using Parser;

    public sealed class CssReference : CssNode
    {
        public CssReference(string name)
            : base(NodeKind.Reference)
        {
            Name = name;
        }

        public CssReference(CssToken name)
            : base(NodeKind.Assignment)
        {
            Name = name.Text;
        }

        public string Name { get; }
    }
}