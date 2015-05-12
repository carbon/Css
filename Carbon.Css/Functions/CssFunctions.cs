using System;
using System.Collections.Generic;

namespace Carbon.Css
{
	using Color;

	public static class CssFunctions
	{
		private static readonly Dictionary<string, Func<CssValue[], CssValue>> dic = new Dictionary<string, Func<CssValue[], CssValue>>(6) {
			["darken"]		= Darken,
			["lighten"]		= Lighten,
			["saturate"]	= Saturate,
			["desaturate"]	= Desaturate,
			["adjust-hue"]	= AdjustHue,
			["mix"]			= Mix
		};

		public static bool TryGet(string name, out Func<CssValue[], CssValue> func)
		{
			return dic.TryGetValue(name, out func);
		}

		public static CssValue Mix(CssValue[] args)
		{
			var color1 = GetColor(args[0]);
			var color2 = GetColor(args[1]);

			var amount = 0.5f;

			if (args.Length == 3)
			{
				amount = GetAmount(args[2]);
			}

			return new CssColor(color1.BlendWith(color2, amount));
		}

		public static CssValue Saturate(CssValue[] args)
		{
			var color = GetColor(args[0]);
			var amount = GetAmount(args[1]);

			return new CssColor(color.Saturate(amount));
		}

		public static CssValue Desaturate(CssValue[] args)
		{
			var color	= GetColor(args[0]);
			var amount	= GetAmount(args[1]);

			return new CssColor(color.Desaturate(amount));
		}

		public static CssValue Lighten(CssValue[] args)
		{
			var color = GetColor(args[0]);
			var amount = GetAmount(args[1]);

			return new CssColor(color.Lighten(amount));
		}

		public static CssValue Darken(CssValue[] args)
		{
			var color = GetColor(args[0]);
			var amount = GetAmount(args[1]);

			return new CssColor(color.Darken(amount));
		}

		public static CssValue AdjustHue(CssValue[] args)
		{
			var color = GetColor(args[0]);
			var amount = GetAmount(args[1]);

			return new CssColor(color.ToHsla().RotateHue(amount * 360).ToRgb());
		}

		public static CssValue Rgba(CssValue[] args)
		{
			var color = Color.Rgba.Parse(args[0].ToString());

			return CssColor.FromRgba(color.R, color.G, color.B, float.Parse(args[1].ToString()));
		}

		private static Rgba GetColor(CssValue value)
		{
			return Color.Rgba.Parse(value.ToString());
		}

		private static float GetAmount(CssValue value)
		{
			// TODO: consider value.kind

			var text = value.ToString();

			if (text.EndsWith("deg"))
			{
				return (float.Parse(text.Replace("deg", "")) % 360) / 360;
			}
			else if (text.EndsWith("%"))
			{
				return float.Parse(text.TrimEnd('%')) / 100;
			}
			else
			{
				return float.Parse(text);
			}
		}
	}
}