namespace Carbon.Css
{
	using Carbon.Css.Parser;

	public class CssNumber : CssValue
	{
		private string text;

		public CssNumber(CssToken token)
			: this(token.Text)
		{ }

		public CssNumber(string text)
			: base(NodeKind.Number) 
		{ 
			this.text = text;
		}
		
		public override string Text
		{
			get { return text; }
		}

		public float ToInt()
		{
			return int.Parse(text);
		}

		public float ToFloat()
		{
			return float.Parse(text);
		}

		public double ToDouble()
		{
			return double.Parse(text);
		}

		public override CssNode CloneNode()
		{
			return new CssNumber(text);
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