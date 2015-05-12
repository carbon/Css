namespace Carbon.Color
{
	using static System.Math;

	public static class ColorExtensions
	{
		public static Readability CalculateReadability(this Rgba a, Rgba b)
		{
			var brightnessA = (a.R * 299 + a.G * 587 + a.B * 114) / 1000;
			var brightnessB = (b.R * 299 + b.G * 587 + b.B * 114) / 1000;

			var colorDiff = (
				Max(a.R, b.R) - Min(a.R, b.R) +
				Max(a.G, b.G) - Min(a.G, b.G) +
				Max(a.B, b.B) - Min(a.B, b.B)
			);

			return new Readability(
				brightness : Abs(brightnessA - brightnessB),
				color      :  colorDiff
			);
		}

		/// <summary>
		/// Computes the Euclidean distance between two colors in the RGB color space.
		/// </summary>
		public static double CalculateSimilarity(this Rgba color1, Rgba color2)
		{
			return Sqrt(
				Pow((color2.R - color1.R), 2) +
				Pow((color2.G - color1.G), 2) +
				Pow((color2.B - color1.B), 2)
			);
		}
	}
}