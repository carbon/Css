using System;
using System.IO;

using Carbon.Color;

namespace Carbon.Css;

public sealed class CssColor : CssValue
{
    private readonly Rgba32? _colorValue;
    private readonly string? _stringValue;

    public CssColor(string value)
        : base(NodeKind.Color)
    {
        _stringValue = value;
    }

    public CssColor(Rgba32 value)
        : base(NodeKind.Color)
    {
        _colorValue = value;
    }

    internal override void WriteTo(TextWriter writer)
    {
        writer.Write(ToString());
    }

    public override string ToString()
    {
        return _colorValue.HasValue
            ? _colorValue.ToString()!
            : _stringValue!;
    }

    public static CssColor FromRgba(byte r, byte g, byte b, float a)
    {
        return new CssColor(FormattableString.Invariant($"rgba({r}, {g}, {b}, {a})"));
    }

    public override CssColor CloneNode()
    {
        return _colorValue.HasValue
            ? new CssColor(_colorValue.Value)
            : new CssColor(_stringValue!);
    }
}

// hsl
// rgba
// hsla
// #hex
// named (purple, yellow, orange)