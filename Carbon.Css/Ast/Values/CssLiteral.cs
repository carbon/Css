namespace Carbon.Css
{
	using Carbon.Css.Parser;

	public class CssLiteral : CssValue
	{
		private string text;

		public CssLiteral(CssToken token)
			: this(token.Text)
		{ }

		public CssLiteral(string text)
			: base(NodeKind.Literal) 
		{ 
			this.text = text;
		}

		public override string Text
		{
			get { return text; }
		}

		public override string ToString()
		{
			return Text;
		}
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