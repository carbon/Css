namespace Carbon.Css
{
    using Parser;

    public sealed class CssVariable : CssValue
    {
        public CssVariable(CssToken token)
            : this(token.Text)
        { }

        public CssVariable(string text)
            : base(NodeKind.Variable)
        {
            Symbol = text;
        }

        public string Symbol { get; }

        public override CssNode CloneNode() => new CssVariable(Symbol);

        public override string ToString() => "$" + Symbol;
    }
}

// Variable (mathematics), a symbol that represents a quantity in a mathematical expression