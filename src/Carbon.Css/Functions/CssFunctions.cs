using System;
using System.Collections.Generic;
using System.Globalization;
using System.Diagnostics.CodeAnalysis;

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

	public static bool TryGet(string name, [NotNullWhen(true)] out Func<CssValue[], CssValue>? func)
	{
		return dic.TryGetValue(name, out func);
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
        Rgba32 color1 = GetColor(args[0]);
        Rgba32 color2 = GetColor(args[1]);
            
        double amount = args.Length == 3 ? GetAmount(args[2]) : 0.5;

		return new CssColor(color1.BlendWith(color2, (float)amount));
	}

	public static CssValue Saturate(CssValue[] args)
	{
        Rgba32 color = GetColor(args[0]);
		var amount = GetAmount(args[1]);

		return new CssColor(color.Saturate((float)amount));
	}

	public static CssValue Desaturate(CssValue[] args)
	{
        Rgba32 color	= GetColor(args[0]);
		var amount	= GetAmount(args[1]);

		return new CssColor(color.Desaturate((float)amount));
	}

	public static CssValue Lighten(CssValue[] args)
	{
        Rgba32 color = GetColor(args[0]);
		var amount = GetAmount(args[1]);

		return new CssColor(color.Lighten((float)amount));
	}

	public static CssValue Darken(CssValue[] args)
	{
        Rgba32 color = GetColor(args[0]);
		var amount = GetAmount(args[1]);

		return new CssColor(color.Darken((float)amount));
	}

	public static CssValue AdjustHue(CssValue[] args)
	{
        Rgba32 color = GetColor(args[0]);
		var amount = GetAmount(args[1]);

		return new CssColor(color.ToHsla().RotateHue((float)amount * 360).ToRgba32());
	}

	public static CssValue Rgba(CssValue[] args)
	{
		if (args.Length == 4)
		{
			return new CssFunction("rgba", new CssValueList(args));
		}

		string arg_0 = args[0].ToString()!;
		string arg_1 = args[1].ToString()!;

		var color = Rgba32.Parse(arg_0);
			
		return CssColor.FromRgba(color.R, color.G, color.B, float.Parse(arg_1, CultureInfo.InvariantCulture));
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

	private static Rgba32 GetColor(CssValue value)
	{
		return Rgba32.Parse(value.ToString()!);
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
		_                   => throw new Exception("Unknown numeric value: " + value.Kind + ":" +  value)
	};
		
#endregion
}