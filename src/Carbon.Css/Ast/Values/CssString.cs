using System.IO;

using Carbon.Css.Parser;

namespace Carbon.Css;

public sealed class CssString : CssValue
{
    public CssString(CssToken token)
        : this(token.Text)
    { }

    internal CssString(CssToken token, Trivia? trailing)
       : this(token.Text)
    {
        this.Trailing = trailing;
    }

    public CssString(string text)
        : base(NodeKind.String)
    {
        Text = text;
    }

    public string Text { get; }

    public override CssString CloneNode() => new(Text);

    internal override void WriteTo(TextWriter writer)
    {
        writer.Write(Text);
    }

    public override string ToString() => Text;
}

/*
math    : calc S*;
calc    : "calc(" S* sum S* ")";
sum     : product [ S+ [ "+" | "-" ] S+ product ]*;
product : unit [ S* [ "*" S* unit | "/" S* NUMBER ] ]*;
attr    : "attr(" S* qname [ S+ type-keyword ]? S* [ "," [ unit | calc ] S* ]? ")";
unit    : [ NUMBER | DIMENSION | PERCENTAGE | "(" S* sum S* ")" | calc | attr ];
*/
