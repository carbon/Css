namespace Carbon.Css;

using Parser;

public sealed class CssVariable(string text) : CssValue(NodeKind.Variable)
{
    public CssVariable(CssToken token)
        : this(token.Text)
    { }

    public string Symbol { get; } = text;

    public override CssVariable CloneNode() => new(Symbol);

    public override string ToString() => "$" + Symbol;
}

// Variable (mathematics), a symbol that represents a quantity in a mathematical expression