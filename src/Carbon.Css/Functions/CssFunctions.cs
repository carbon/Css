using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using Carbon.Color;

namespace Carbon.Css;

public static class CssFunctions
{
    // http://lesscss.org/functions/#color-operations-saturate

    private static readonly Dictionary<string, Func<CssValue[], CssValue>> dic = new (10) {
        ["darken"]		= Darken,
        ["lighten"]		= Lighten,
        ["saturate"]	= Saturate,
        ["desaturate"]	= Desaturate,
        ["adjust-hue"]	= AdjustHue,
        ["unquote"]     = Unquote,
        ["mix"]			= Mix,
        ["rgba"]		= Rgba,
        ["if"]			= If
    };

    // red(color)
    // green(color)
    // blue(blue)
    // hsl
    // hsla
    // hue

    public static bool TryGet(string name, [NotNullWhen(true)] out Func<CssValue[], CssValue>? function)
    {
        return dic.TryGetValue(name, out function);
    }

    // if($condition, $if-true, $if-false)
    public static CssValue If(CssValue[] args)
    {
        return ToBoolean(args[0]) ? args[1] : args[2];
    }

    public static CssValue Unquote(CssValue[] args)
    {
        string value = args[0].ToString()!;

        return new CssString(value.Trim('"'));
    }

    public static CssValue Mix(CssValue[] args)
    {
        var color1 = GetColor(args[0]).ToRgba32();
        var color2 = GetColor(args[1]).ToRgba32();

        double amount = args.Length == 3 ? GetAmount(args[2]) : 0.5;

        return new CssColor(color1.BlendWith(color2, (float)amount));
    }

    public static CssValue Saturate(CssValue[] args)
    {
        Hsla color = GetColor(args[0]).ToHsla();
        var amount = GetAmount(args[1]);

        return new CssColor(color.AdjustSaturation((float)amount));
    }

    public static CssValue Desaturate(CssValue[] args)
    {
        Hsla color = GetColor(args[0]).ToHsla();
        var amount = GetAmount(args[1]);

        return new CssColor(color.AdjustSaturation(-(float)amount));
    }

    public static CssValue Lighten(CssValue[] args)
    {
        Hsla color = GetColor(args[0]).ToHsla();
        var amount = GetAmount(args[1]);

        return new CssColor(color.AdjustLightness((float)amount));
    }

    public static CssValue Darken(CssValue[] args)
    {
        var color = GetColor(args[0]).ToHsla();
        var amount = GetAmount(args[1]);

        return new CssColor(color.AdjustLightness(-(float)amount));
    }

    public static CssValue AdjustHue(CssValue[] args)
    {
        Hsla color = GetColor(args[0]).ToHsla();
        var amount = GetAmount(args[1]);

        return new CssColor(color.RotateHue((float)amount * 360).ToRgba32());
    }

    public static CssValue Rgba(CssValue[] args)
    {
        if (args.Length is 2 && args[1] is CssUnitValue opacity)
        {
            var color = CssColor.Parse(args[0].ToString()).Value;

            double alpha = opacity.Value;

            if (opacity.Kind is NodeKind.Percentage)
            {
                alpha /= 100d;
            }

            return new CssColor(new Rgba128f(color.R, color.G, color.B, (float)alpha));
        }
        else
        {
            return new CssFunction("rgba", new CssValueList(args));
        }
    }

    #region Helpers

    private static bool ToBoolean(CssValue value)
    {
        if (value is CssBoolean cssBoolean)
        {
            return cssBoolean.Value;
        }

        return string.Equals(value.ToString()!, "true", StringComparison.OrdinalIgnoreCase);
    }

    private static Rgba128f GetColor(CssValue value)
    {
        if (value is CssColor { Value: Rgba128f v })
        {
            return v;
        }

        return Rgba128f.FromRgba32(Rgba32.Parse(value.ToString()!));
    }

    private static double ParseDouble(ReadOnlySpan<char> text)
    {
        if (text.EndsWith("deg", StringComparison.Ordinal))
        {
            text = text[0..^3];
        }

        return double.Parse(text, provider: CultureInfo.InvariantCulture);
    }

    private static double GetAmount(CssValue value) => value.Kind switch
    {
        NodeKind.Angle      => (ParseDouble(value.ToString().AsSpan()) % 360) / 360,
        NodeKind.Percentage => (((CssUnitValue)value).Value / 100d),
        NodeKind.Number     => ((CssUnitValue)value).Value,
        _                   => throw new Exception($"Unknown numeric value. Was {value.Kind}:{value}")
    };
        
#endregion
}