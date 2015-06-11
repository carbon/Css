﻿namespace Carbon.Css
{
	public class CssMeasurement : CssValue
	{
		private readonly float number;
		private readonly CssUnit unit;

		public CssMeasurement(float number, CssUnit unit)
			: base(unit.Kind)
		{
			this.number = number;
			this.unit	= unit;
		}

		public float Number => number;

		public CssUnit Unit => unit;

		public override string ToString() => number + unit.Name;

		public override CssNode CloneNode() => new CssMeasurement(number, unit);
	}

	// CssLength
	// CssTime
	// ...
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