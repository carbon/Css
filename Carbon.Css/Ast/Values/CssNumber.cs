namespace Carbon.Css
{
	using Parser;

	public class CssNumber : CssValue
	{
		private readonly string text;

		public CssNumber(CssToken token)
			: this(token.Text)
		{ }

		public CssNumber(string text)
			: base(NodeKind.Number) 
		{ 
			this.text = text;
		}
		
		public float ToInt() => int.Parse(text);

		public float ToFloat() => float.Parse(text);

		public override CssNode CloneNode() => new CssNumber(text);

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