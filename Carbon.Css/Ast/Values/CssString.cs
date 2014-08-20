namespace Carbon.Css
{
	using Carbon.Css.Parser;

	public class CssString : CssValue
	{
		private string text;

		public CssString(CssToken token)
			: this(token.Text)
		{ }

		public CssString(string text)
			: base(NodeKind.String) 
		{ 
			this.text = text;
		}

		public override CssNode Clone()
		{
			return new CssString(text);
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