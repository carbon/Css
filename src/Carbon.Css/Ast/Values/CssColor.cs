using System.IO;

using Carbon.Color;
using Carbon.Css.Helpers;

namespace Carbon.Css;

public sealed class CssColor : CssValue
{
    // | color(rec2020 0.42053 0.979780 0.00579)

    // private readonly CssColorType type;
    private readonly Rgba128f? _value; // Vector4
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

    public static new CssColor Parse(ReadOnlySpan<char> text)
    {
        int pIndex = text.IndexOf('(');

        if (pIndex > -1)
        {
            var name   = text[0..pIndex];
            var values = text[(pIndex + 1)..^1];

            float w = 1; // alpha

            int slashIndex = values.IndexOf('/');

            if (slashIndex > -1)
            {
                w = NumberHelper.ParseCssNumberAsF32(values[(slashIndex + 1)..].Trim());

                values = values[0..slashIndex];

            }

            char separator = values.Contains(',') ? ',' : ' ';

            var splitter = new StringSplitter(values, separator);

            splitter.TryGetNextF32(out float x);
            splitter.TryGetNextF32(out float y);
            splitter.TryGetNextF32(out float z);

            if (!splitter.IsEof)
            {
                splitter.TryGetNextF32(out w);
            }

            return name switch
            {
                "rgb" or "rgba" => new CssColor(new Rgba128f(x / 255f, y / 255f, z / 255f, w)),
                // "hls" or "hsla":
                // "hwb":
                // "lab":
                // "lch":
                // "oklab":
                // "oklch":
                //     throw new Exception("unsupported color type");
                _ => throw new Exception($"unsupported color type {name}")
            };
        }

        // | rgb(146.064 107.457 131.223)
        // | rgba
        // | hsl
        // | hsla
        // | hwb
        // | lab(29.69% 44.888% -29.04%)
        // | lch(60.2345 59.2 95.2)
        // | oklab(40.101% 0.1147 0.0453)
        // | oklch(78.3% 0.108 326.5)

        return new CssColor(Rgba32.Parse(text));
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