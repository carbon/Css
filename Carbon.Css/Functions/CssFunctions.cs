namespace Carbon.Css
{
	using System;
	using System.Collections.Generic;

	using Carbon.Css.Color;

	public static class CssFunctions
	{
		public static bool TryGet(string name, out Func<CssValue[], CssValue> func)
		{
			switch (name)
			{
				case "darken"		: func = Darken;		return true;
				case "lighten"		: func = Lighten;		return true;
				case "saturate"		: func = Saturate;		return true;
				case "desaturate"	: func = Desaturate;	return true;
				case "adjust-hue"	: func = AdjustHue;		return true;
			}

			func = null;

			return false;
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

			return new CssColor(color.ToHsla().AdjustHue(amount).ToRgb());
		}

		public static CssValue Rgba(CssValue[] args)
		{
			var color = WebColor.Parse(args[0].ToString());

			return CssColor.FromRgba(color.R, color.G, color.B, float.Parse(args[1].ToString()));
		}


		private static WebColor GetColor(CssValue value)
		{
			return WebColor.Parse(value.ToString());
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
