namespace Carbon.Css
{
    using Parser;

    public class CssVariable : CssValue
    {
        private readonly string text;

        public CssVariable(CssToken token)
            : this(token.Text)
        { }

        public CssVariable(string text)
            : base(NodeKind.Variable)
        {
            this.text = text;
        }

        public string Symbol => text;

        public override CssNode CloneNode() => new CssVariable(text);

        public override string ToString() => "$" + text;
    }
}

// Variable (mathematics), a symbol that represents a quantity in a mathematical expression