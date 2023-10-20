using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using Carbon.Css.Ast;
using Carbon.Css.Helpers;
using Carbon.Css.Serialization;

namespace Carbon.Css;

[JsonConverter(typeof(CssUnitValueConverter))]
public sealed class CssUnitValue(double value, CssUnitInfo unit) 
    : CssValue(unit.Kind), IEquatable<CssUnitValue>, ICssNumericValue, ISpanFormattable
{
    public static readonly CssUnitValue Zero = new(0, CssUnitInfo.Number);

    public CssUnitValue(double value, string unitName)
        : this(value, CssUnitInfo.Get(unitName)) { }

    public double Value { get; } = value;

    public CssUnitInfo Unit { get; } = unit;

    internal override void WriteTo(scoped ref ValueStringBuilder sb)
    {
        sb.AppendInvariant(Value);
        sb.Append(Unit.Name);
    }

    [SkipLocalsInit]
    internal override void WriteTo(TextWriter writer)
    {
        Span<char> buffer = stackalloc char[16];

        if (Value.TryFormat(buffer, out int charsWritten, provider: CultureInfo.InvariantCulture))
        {
            writer.Write(buffer.Slice(0, charsWritten));
        }
        else
        {
            writer.Write(Value.ToString(CultureInfo.InvariantCulture));
        }

        writer.Write(Unit.Name);
    }

    public override CssUnitValue CloneNode() => new(Value, Unit);

    public new static CssUnitValue Parse(string text)
    {
        return Parse(text.AsSpan());
    }

    public static CssUnitValue Parse(scoped ReadOnlySpan<char> text)
    {
        double value = NumberHelper.ReadNumber(text, out int read);

        if (read == text.Length)
        {
            return Number(value);
        }

        return new CssUnitValue(value, CssUnitInfo.Get(text[read..].Trim()));
    }

    public static CssUnitValue Parse(ReadOnlySpan<byte> utf8Bytes)
    {
        double value = NumberHelper.ReadNumber(utf8Bytes, out int read);

        if (read == utf8Bytes.Length)
        {
            return Number(value);
        }

        var suffixBytes = utf8Bytes[read..].Trim((byte)' ');

        return new CssUnitValue(value, CssUnitInfo.Get(suffixBytes));
    }

    #region Operators

    // Subtract
    // Div
    // Min
    // Max

    public CssUnitValue Multiply(CssValue other)
    {
        if (other.Kind is NodeKind.Percentage)
        {
            return new CssUnitValue(Value * (((CssUnitValue)other).Value / 100), Unit);
        }
        else if (other.Kind is NodeKind.Number)
        {
            return new CssUnitValue(Value * (((CssUnitValue)other).Value), Unit);
        }
        else if (other is CssUnitValue measurement)
        {
            return new CssUnitValue(Value * measurement.Value, measurement.Unit);
        }

        throw new Exception($"{Kind} and {other.Kind} are not compatible | {this} * {other}");
    }

    public CssUnitValue Divide(CssValue other)
    {
        if (other.Kind is NodeKind.Percentage or NodeKind.Number)
        {
            return new CssUnitValue(Value / (((CssUnitValue)other).Value), Unit);
        }
        else if (other is CssUnitValue otherUnit)
        {
            if (other.Kind == Kind || other.Kind is NodeKind.Number)
            {
                return new CssUnitValue(Value / otherUnit.Value, otherUnit.Unit);
            }
        }

        throw new Exception($"{Kind} and {other.Kind} are not compatible | | {this} / {other}");
    }

    public CssUnitValue Add(CssValue other)
    {
        if (other is CssUnitValue measurement && measurement.Kind == Kind)
        {
            return new CssUnitValue(Value + measurement.Value, measurement.Unit);
        }

        throw new Exception($"{Kind} and {other.Kind} are not compatible | {this} + {other}");
    }

    public CssUnitValue Subtract(CssValue other)
    {
        if (other is CssUnitValue measurement && measurement.Kind == Kind)
        {
            return new CssUnitValue(Value - measurement.Value, measurement.Unit);
        }

        throw new Exception($"{Kind} and {other.Kind} are not compatible | {this} - {other}");
    }

    #endregion

    public void Deconstruct(out double value, out CssUnitInfo unit)
    {
        (value, unit) = (Value, Unit);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Unit, Value);
    }

    public bool Equals(CssUnitValue? other)
    {
        if (other is null) return this is null;

        if (ReferenceEquals(this, other)) return true;

        return Unit.Equals(other.Unit)
            && Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return obj is CssUnitValue other && Equals(other);
    }

    public override string ToString()
    {
        return string.Create(CultureInfo.InvariantCulture, $"{Value}{Unit.Name}");
    }

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider? provider = null)
    {
        return destination.TryWrite(CultureInfo.InvariantCulture, $"{Value}{Unit.Name}", out charsWritten);
    }

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return string.Create(CultureInfo.InvariantCulture, $"{Value}{Unit.Name}");
    }
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
