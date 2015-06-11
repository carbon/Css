namespace Carbon.Css
{
	public class CssNumber : CssValue
	{
		private readonly float value;

		public CssNumber(float value)
			: base(NodeKind.Number) 
		{ 
			this.value = value;
		}
		
		public float Value => value;

		public override CssNode CloneNode() => new CssNumber(value);

		public override string ToString() => value.ToString();
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