namespace Carbon.Css
{
    using Parser;

    public class CssString : CssValue
    {
        public CssString(CssToken token)
            : this(token.Text)
        { }

        public CssString(string text)
            : base(NodeKind.String)
        {
            Text = text;
        }

        public string Text { get; }

        public override CssNode CloneNode() => new CssString(Text);

        public override string ToString() => Text;
    }
}

/*
math    : calc S*;
calc    : "calc(" S* sum S* ")";
sum     : product [ S+ [ "+" | "-" ] S+ product ]*;
product : unit [ S* [ "*" S* unit | "/" S* NUMBER ] ]*;
attr    : "attr(" S* qname [ S+ type-keyword ]? S* [ "," [ unit | calc ] S* ]? ")";
unit    : [ NUMBER | DIMENSION | PERCENTAGE | "(" S* sum S* ")" | calc | attr ];
*/
