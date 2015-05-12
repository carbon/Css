namespace Carbon.Color
{
	using System.Collections.Generic;

	// Based ON
	// TinyColor v1.1.2 https://github.com/bgrins/TinyColor Brian Grinstead, MIT License

	public class ReadabilityCalculator
	{
		public static ReadableColor CalculateMostReadable(Rgba baseColor, IEnumerable<Rgba> colorList)
		{
			ReadableColor bestColor = null;

			var bestScore = 0d;

			foreach (var color in colorList)
			{
				// rank brightness constrast higher than hue.

				var readability = baseColor.CalculateReadability(color);

				var readable = readability.Brightness > 125 && readability.Color > 500;

				var score = 3 * (readability.Brightness / 125) + (readability.Color / 500);

				if (score > bestScore)
				{
					bestScore = score;

					bestColor = new ReadableColor(color, readability);
				}
			}

			return bestColor;
		}
	}
}
