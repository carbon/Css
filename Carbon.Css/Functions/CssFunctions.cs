using System;
using System.Collections.Generic;

namespace Carbon.Css
{
	using Color;

	public static class CssFunctions
	{
		// http://lesscss.org/functions/#color-operations-saturate

		private static readonly Dictionary<string, Func<CssValue[], CssValue>> dic = new Dictionary<string, Func<CssValue[], CssValue>>(9) {
			["darken"]		    = Darken,
			["lighten"]		    = Lighten,
			["saturate"]	    = Saturate,
			["desaturate"]	    = Desaturate,
			["adjust-hue"]	    = AdjustHue,
			["mix"]			    = Mix,
			["rgba"]		    = Rgba,
			["readability"]     = Readability,

            ["if"]			    = If
        };

		public static bool TryGet(string name, out Func<CssValue[], CssValue> func)
		    => dic.TryGetValue(name, out func);
		
		// if($condition, $if-true, $if-false)
		public static CssValue If(CssValue[] args)
		    => (ToBoolean(args[0])) ? args[1] : args[2];

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
			if (args.Length == 4)
			{
				return new CssFunction("rgba", new CssValueList(args));
			}

			var color = Color.Rgba.Parse(args[0].ToString());
			
			return CssColor.FromRgba(color.R, color.G, color.B, float.Parse(args[1].ToString()));
		}

		public static CssValue Readability(CssValue[] args)
		{ 
			var color1 = GetColor(args[0]);
			var color2 = GetColor(args[1]);

			return new CssNumber((float)color1.CalculateReadability(color2).Color);
		}

        #region Helpers

        private static bool ToBoolean(CssValue value)
        {
            if (value.Kind == NodeKind.Boolean) return ((CssBoolean)value).Value;

            return value.ToString().ToLower() == "true";
        }

		private static Rgba GetColor(CssValue value)
		    => Color.Rgba.Parse(value.ToString());

		private static float GetAmount(CssValue value)
		{
			// TODO: consider value.kind

			var text = value.ToString();

			switch (value.Kind)
			{
				case NodeKind.Angle      : return (float.Parse(text.Replace("deg", "")) % 360) / 360;
				case NodeKind.Percentage : return ((CssMeasurement)value).Value / 100;
				case NodeKind.Number     : return ((CssNumber)value).Value;

				default: throw new Exception("Unknown numeric value:" + value.ToString());
			}
		}

		#endregion
	}
}