namespace Carbon.Css
{
	using Carbon.Css.Color;
	using System.Collections.Generic;

	public class LightenFunction
	{
		public string Execute(CssValue[] args)
		{
			var color = WebColor.Parse(args[0].ToString());
			var amount = args[1].ToString();
			float value = 0;

			if (amount.EndsWith("%"))
			{
				value = float.Parse(amount.TrimEnd('%')) / 100;
			}
			else
			{
				value = float.Parse(amount);
			}

			return color.Lighten(value).ToString();
		}
	}
}
