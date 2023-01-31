using System.Globalization;
using System.IO;

using Carbon.Color;

namespace Carbon.Css;

public sealed class CssColor : CssValue
{
    private readonly Rgba128f? _value;
    private readonly string? _text;

    public CssColor(string value)
        : base(NodeKind.Color)
    {
        _text = value;
    }

    public CssColor(Rgba32 value)
        : base(NodeKind.Color)
    {
        _value = Rgba128f.FromRgba32(value);
    }

    public CssColor(Hsla value)
        : base(NodeKind.Color)
    {
        _value = value.ToRgba128f();
    }

    public CssColor(Rgba128f value)
        : base(NodeKind.Color)
    {
        _value = value;
    }

    public Rgba128f? Value => _value;

    internal override void WriteTo(TextWriter writer)
    {
        writer.Write(ToString());
    }

    public override string ToString()
    {
        if (_text != null) return _text;

        if (_value is null) return null!;

        var rgba32 = _value.Value.ToRgba32();

        if (rgba32.IsOpaque)
        {
            return rgba32.ToString();
        }

        return rgba32.ToCssString();


    }

    public static CssColor FromRgb(byte r, byte g, byte b, float a = 1)
    {
        return new CssColor(new Rgba128f(r / 255f, g / 255f, b / 255f, a));
    }

    public static new CssColor Parse(string text)
    {
        return new CssColor(Color.Rgba32.Parse(text));
    }

    public override CssColor CloneNode()
    {
        return _value.HasValue
            ? new CssColor(_value.Value)
            : new CssColor(_text!);
    }
}

// hsl
// rgba
// hsla
// #hex
// named (purple, yellow, orange)