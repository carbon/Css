namespace Carbon.Css
{
	using Parser;

	public class CssString : CssValue
	{
		private readonly string text;

		public CssString(CssToken token)
			: this(token.Text)
		{ }

		public CssString(string text)
			: base(NodeKind.String) 
		{ 
			this.text = text;
		}

		public string Text => text;

		public override CssNode CloneNode() => new CssString(text);

		public override string ToString() => text;
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