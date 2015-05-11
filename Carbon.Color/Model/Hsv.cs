namespace Carbon.Color
{
	using System.Linq;

	public struct Hsv
	{
		private readonly float h;
		private readonly float s;
		private readonly float v;

		public Hsv(float h, float s, float v)
		{
			this.h = h;
			this.s = s;
			this.v = v;
		}

		public double H => h;
		public double S => s;
		public double V => v;

		public static Hsv FromColor(WebColor color)
		{
			// Clamp to 0-1
			float r = color.R / 255f,
				  b = color.B / 255f,
				  g = color.G / 255f;

			var max = new[] { r, g, b }.Max();
			var min = new[] { r, g, b }.Min();
			var d = max - min;

			float h = 0f,
				  s = 0f,
				  v = max;

			if (max != 0) s = d / max;

			if (max == min)
			{
				h = 0f;
			}
			else
			{
				if (max == r)		h = (g - b) / d + (g < b ? 6 : 0);
				else if (max == g)	h = (b - r) / d + 2; 
				else if (max == b)	h = (r - g) / d + 4;

				h /= 6f;
			}

			return new Hsv(h, s, v);
		}
    }
}
