namespace Carbon.Color
{
	public class ReadableColor
	{
		private readonly Readability readability;
		private readonly Rgba color;

		public ReadableColor(Rgba color, Readability readability)
		{
			this.color = color;
			this.readability = readability;
		}

		public Rgba Color => color;

		public Readability Readability => readability;
	}

	public struct Readability
	{
		private readonly double brightness;
		private readonly double color;

		public Readability(double brightness, double color)
		{
			this.brightness = brightness;
			this.color = color;
		}

		public double Brightness => brightness;

		public double Color => color;
	}
}