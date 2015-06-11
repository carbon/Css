namespace Carbon.Css
{
	using System;

	public class CssMeasurement : CssValue
	{
		private readonly float value;
		private readonly CssUnit unit;

		public CssMeasurement(float value, CssUnit unit)
			: base(unit.Kind)
		{
			this.value = value;
			this.unit	= unit;
		}

		public float Value => value;

		public CssUnit Unit => unit;

		public override string ToString() => value + unit.Name;

		public override CssNode CloneNode() => new CssMeasurement(value, unit);

		public CssValue Multiply(CssValue node)
		{
			switch (node.Kind)
			{
				case NodeKind.Number	 : return new CssMeasurement(this.value * ((CssNumber)node).Value, unit);
				case NodeKind.Percentage : return new CssMeasurement(this.value * (((CssNumber)node).Value / 100), unit);
				default					 : throw new Exception($"Cannot multiply {node.Kind} with {this.Kind}.");
			}
		}
	}

	// CssLength
	// CssTime
	// ...
}

// A dimension is a number immediately followed by a unit identifier.
// It corresponds to the DIMENSION token in the grammar. 
// [CSS21] Like keywords, unit identifiers are case-insensitive within the ASCII range.

/*
math    : calc S*;
calc    : "calc(" S* sum S* ")";
sum     : product [ S+ [ "+" | "-" ] S+ product ]*;
product : unit [ S* [ "*" S* unit | "/" S* NUMBER ] ]*;
attr    : "attr(" S* qname [ S+ type-keyword ]? S* [ "," [ unit | calc ] S* ]? ")";
unit    : [ NUMBER | DIMENSION | PERCENTAGE | "(" S* sum S* ")" | calc | attr ];
*/

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
