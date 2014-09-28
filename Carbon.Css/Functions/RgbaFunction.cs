namespace Carbon.Css
{
	using Carbon.Css.Color;
	using System.Collections.Generic;

	public class RgbaFunction : ICssFunction
	{
		public CssValue Execute(CssValue[] args)
		{
			var color = WebColor.Parse(args[0].ToString());

			return CssColor.FromRgba(color.R, color.G, color.B, float.Parse(args[1].ToString()));
		}
	}
}
