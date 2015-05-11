namespace Carbon.Css
{
	using Parser;

	public class CssDimension : CssValue
	{
		private readonly CssToken number;
		private readonly CssToken unit;

		public CssDimension(CssToken number, CssToken unit)
			: base(GetDimensionKind(unit.Text))
		{
			this.number = number;
			this.unit	= unit;
		}

		public override string ToString() => number.Text + unit.Text;

		public override CssNode CloneNode() => new CssDimension(number, unit);

		public static NodeKind GetDimensionKind(string a)
		{
			switch (a)
			{
				case "cm": 
				case "mm": 
				case "in":
				case "pt": 
				case "pc": 
				case "px": return NodeKind.Length;

				case "deg"  :
				case "rad"  :
				case "grad" : return NodeKind.Angle;

				case "ms" :
				case "s"  : return NodeKind.Time;

				case "hz"  :
				case "khz" : return NodeKind.Frequency;

				case "%"   : return NodeKind.Percent;

				default	   : return NodeKind.Dimension;
			}
		}
	}
}

// A dimension is a number immediately followed by a unit identifier.
// It corresponds to the DIMENSION token in the grammar. 
// [CSS21] Like keywords, unit identifiers are case-insensitive within the ASCII range.

/*
{num}{E}{M}			{return EMS;}
{num}{E}{X}			{return EXS;}
{num}{P}{X}			{return LENGTH;}
{num}{C}{M}			{return LENGTH;}
{num}{M}{M}			{return LENGTH;}
{num}{I}{N}			{return LENGTH;}
{num}{P}{T}			{return LENGTH;}
{num}{P}{C}			{return LENGTH;}
{num}{D}{E}{G}		{return ANGLE;}
{num}{R}{A}{D}		{return ANGLE;}
{num}{G}{R}{A}{D}	{return ANGLE;}
{num}{M}{S}			{return TIME;}
{num}{S}			{return TIME;}
{num}{H}{Z}			{return FREQ;}
{num}{K}{H}{Z}		{return FREQ;}
{num}{ident}		{return DIMENSION;}

{num}%				{return PERCENTAGE;}
{num}				{return NUMBER;}
*/


/*
EMS,
EXS,
PX,
CM,
MM,
IN,
PT,
PC,
DEG,
RAD,
GRAD,
TURN
*/