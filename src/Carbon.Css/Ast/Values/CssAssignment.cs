namespace Carbon.Css
{
    using Parser;

    public class CssAssignment : CssNode
    {
        public CssAssignment(string name, CssValue value)
            : base(NodeKind.Assignment)
        {
            Name = name;
            Value = value;
        }

        public CssAssignment(CssToken name, CssValue value)
            : base(NodeKind.Assignment)
        {
            Name = name.Text;
            Value = value;
        }

        public string Name { get; }

        public CssValue Value { get; }
    }
}