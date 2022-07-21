#pragma warning disable IDE0057 // Use range operator

using System;
using System.Runtime.Serialization;

namespace Carbon.Css;

[DataContract]
public readonly struct CssPlacement : IEquatable<CssPlacement>
{
    public CssPlacement(CssBoxAlignment value)
    {
        Justify = value;
        Align = value;
    }

    public CssPlacement(CssBoxAlignment align, CssBoxAlignment justify)
    {
        Align = align;
        Justify = justify;
    }

    // Cross Axis : Y (align) | when columns
    [DataMember(Name = "align", Order = 1)]
    public readonly CssBoxAlignment Align { get; }

    // Main Axis : X (justify)
    [DataMember(Name = "justify", Order = 2)]
    public readonly CssBoxAlignment Justify { get; }

    public static CssPlacement Parse(string text)
    {
        return Parse(text.AsSpan());
    }

    public static CssPlacement Parse(ReadOnlySpan<char> text)
    {
        // center center
        // top left
        // start end

        int spaceIndex = text.IndexOf(' ');

        if (spaceIndex == -1)
        {
            CssBoxAlignment value = CssBoxAlignmentExtensions.Parse(text);

            return new CssPlacement(value, value);
        }

        var lhs = text[..spaceIndex];
        var rhs = text.Slice(spaceIndex + 1);

        CssBoxAlignment align = CssBoxAlignmentExtensions.Parse(lhs);
        CssBoxAlignment justify = CssBoxAlignmentExtensions.Parse(rhs);

        return new CssPlacement(align, justify);
    }

    // left, center, and right
    // top, center, and bottom

    public readonly bool Equals(CssPlacement other)
    {
        return Justify == other.Justify
            && Align == other.Align;
    }

    public override bool Equals(object? obj)
    {
        return obj is CssPlacement other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Justify, Align);
    }

    public readonly override string ToString()
    {
        if (Align == Justify)
        {
            return Align.Canonicalize();
        }

        return Align.Canonicalize() + " " + Justify.Canonicalize();
    }

    public static bool operator ==(CssPlacement left, CssPlacement right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(CssPlacement left, CssPlacement right)
    {
        return !left.Equals(right);
    }
}


//                      y                  x 
// place-items   : <align-items>   / <justify-items
// place-content : <align-content> / <justify-content>
