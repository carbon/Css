using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;

using Carbon.Color;
using Carbon.Css.Helpers;

namespace Carbon.Css;

public sealed class CssColor : CssValue
{
    private readonly CssColorSpace _type = default;
    private readonly Vector4 _value;
    private readonly string? _text;

    public CssColor(string value)
        : base(NodeKind.Color)
    {
        _text = value;
    }

    public CssColor(Rgba32 value)
        : base(NodeKind.Color)
    {
        var rgb = SRgb.FromRgba32(value);

        _value = Unsafe.As<SRgb, Vector4>(ref rgb);
        _type = CssColorSpace.Rgb;
    }

    public CssColor(Hsla value)
        : base(NodeKind.Color)
    {
        _value = Unsafe.As<Hsla, Vector4>(ref value);
        _type = CssColorSpace.Hsl;
    }

    public CssColor(SRgb value)
        : base(NodeKind.Color)
    {
        _type = CssColorSpace.Rgb;
        _value = Unsafe.As<SRgb, Vector4>(ref value);
    }

    private CssColor(CssColorSpace type, Vector4 value)
        : base(NodeKind.Color)
    {
        _type = type;
        _value = value;
    }

    public SRgb Value
    {
        get
        {
            if (_type is CssColorSpace.Hsl)
            {
                return Unsafe.BitCast<Vector4, Hsla>(_value).ToSRgb();
            }

            return new SRgb(_value);
        }
    }

    internal override void WriteTo(TextWriter writer)
    {
        writer.Write(ToString());
    }

    public override string ToString()
    {
        if (_text != null) return _text;

        if (_type == default) return null!;
  
        return Value.ToRgba32().ToHexString();
    }

    public static CssColor FromRgb(byte r, byte g, byte b, float alpha = 1)
    {
        return new CssColor(new SRgb(r / 255f, g / 255f, b / 255f, alpha));
    }

    public static CssColor Parse(ReadOnlySpan<char> text)
    {
        int pIndex = text.IndexOf('(');

        if (pIndex > -1)
        {
            var name   = text[0..pIndex];
            var values = text[(pIndex + 1)..^1];

            float w = 1; // alpha
            
            char separator = values.Contains(',') ? ',' : ' ';

            if (separator is ' ')
            {
                int slashIndex = values.IndexOf('/');

                if (slashIndex > -1)
                {
                    w = NumberHelper.ParseCssNumberAsF32(values[(slashIndex + 1)..].Trim());

                    values = values[0..slashIndex];
                }
            }

            var splitter = new StringSplitter(values, separator);

            splitter.TryGetNextF32(out float x);
            splitter.TryGetNextF32(out float y);
            splitter.TryGetNextF32(out float z);

            if (!splitter.IsEof)
            {
                splitter.TryGetNextF32(out w);
            }

            return name switch {
                "rgb" or "rgba" => new CssColor(new SRgb(x / 255f, y / 255f, z / 255f, w)),
                "hsl" or "hsla" => new CssColor(new Hsla(x, y, z, w)),
                "hwb"   => new CssColor(CssColorSpace.Hwb, new(x, y, z, w)),
                "lab"   => new CssColor(CssColorSpace.Lab,   new(x, y, z, w)),
                "lch"   => new CssColor(CssColorSpace.Lch,   new(x, y, z, w)),
                "oklab" => new CssColor(CssColorSpace.OkLab, new(x, y, z, w)),
                "oklch" => new CssColor(CssColorSpace.OkLch, new(x, y, z, w)),

                _ => throw new Exception($"unsupported color space. was {name}")
            };
        }

        return new CssColor(Rgba32.Parse(text));
    }

    public override CssColor CloneNode()
    {
        return _type != default
            ? new CssColor(_type, _value)
            : new CssColor(_text!);
    }
}

// <rgb()>    |
// <rgba()>   |
// <hsl()>    |
// <hsla()>   |
// <hwb()>    |
// <lab()>    |
// <lch()>    |
// <oklab()>  |
// <oklch()>  |
// <color()>  

// hsl
// rgba
// hsla
// #hex
// named (purple, yellow, orange)

// Notation | Hex | Functional