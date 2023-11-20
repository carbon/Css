using System.IO;
using System.Text;

using Carbon.Css.Parser;

namespace Carbon.Css;

public sealed class CssString(string text) : CssValue(NodeKind.String)
{
    public CssString(CssToken token)
        : this(token.Text)
    { }

    internal CssString(CssToken token, Trivia? trailing)
       : this(token.Text)
    {
        Trailing = trailing;
    }

    public string Text { get; } = text;

    public override CssString CloneNode() => new(Text);

    internal override void WriteTo(scoped ref ValueStringBuilder sb)
    {
        sb.Append(Text);
    }

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
