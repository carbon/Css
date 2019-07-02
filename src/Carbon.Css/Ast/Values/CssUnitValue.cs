using System;
using System.Globalization;
using Carbon.Css.Helpers;

namespace Carbon.Css
{
    public sealed class CssUnitValue : CssValue, IEquatable<CssUnitValue>
	{
        public static readonly CssUnitValue Zero = new CssUnitValue(0, CssUnitInfo.Number);
        
        public CssUnitValue(double value, string unitName)
            : this(value, CssUnitInfo.Get(unitName))
        {
        }

        public CssUnitValue(double value, CssUnitInfo unit)
            : base(unit.Kind)
        {
            Value = value;
            Unit = unit;
        }

        public double Value { get; }

		public CssUnitInfo Unit { get; }

		public override string ToString() => Value.ToString(CultureInfo.InvariantCulture) + Unit.Name;

		public override CssNode CloneNode() => new CssUnitValue(Value, Unit);

        public static CssUnitValue Parse(ReadOnlySpan<char> text)
        {
            double value = text.ReadNumber(out int read);
            
            if (read == text.Length)
            {
                return Number(value);
            }

            return new CssUnitValue(value, text.Slice(read).Trim().ToString());
        }

		#region Operators

        // Subtract
        // Div
        // Min
        // Max

		public CssValue Multiply(CssValue other)
		{
            if (other.Kind == NodeKind.Percentage)
            {
                return new CssUnitValue(Value * (((CssUnitValue)other).Value / 100), Unit);
            }
            else if (other.Kind == NodeKind.Number)
            {
                return new CssUnitValue(Value * (((CssUnitValue)other).Value), Unit);
            }
            else if (other is CssUnitValue measurement)
            {
                return new CssUnitValue(Value * measurement.Value, measurement.Unit);
            }

            throw new Exception($"{this.Kind} and {other.Kind} are not compatible | {this} * {other}");
        }

        public CssValue Divide(CssValue other)
        {
            if (other.Kind == NodeKind.Percentage || other.Kind == NodeKind.Number)
            {
                return new CssUnitValue(Value / (((CssUnitValue)other).Value), Unit);
            }
            else if (other is CssUnitValue otherUnit)
            {
                if (other.Kind == Kind || other.Kind == NodeKind.Number)
                {
                    return new CssUnitValue(Value / otherUnit.Value, otherUnit.Unit);
                }
            }

            throw new Exception($"{this.Kind} and {other.Kind} are not compatible | | {this} / {other}");
        }

        public CssValue Add(CssValue other)
		{
			if (other is CssUnitValue measurement && measurement.Kind == Kind)
            {
                return new CssUnitValue(Value + measurement.Value, measurement.Unit); 
            }

            throw new Exception($"{this.Kind} and {other.Kind} are not compatible | {this} + {other}");
		}

        public CssValue Subtract(CssValue other)
        {
            if (other is CssUnitValue measurement && measurement.Kind == Kind)
            {
                return new CssUnitValue(Value - measurement.Value, measurement.Unit);
            }

            throw new Exception($"{this.Kind} and {other.Kind} are not compatible | {this} - {other}");
        }

        #endregion

        public void Deconstruct(out double value, out CssUnitInfo unit)
        {
            (value, unit) = (Value, Unit);
        }

        public bool Equals(CssUnitValue other)
        {
            return (Unit.Name, Value) == (other.Unit.Name, other.Value);
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
