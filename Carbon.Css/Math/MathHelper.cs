namespace Carbon.Css
{
	internal static class MathHelper
	{
		public static double Lerp(this double start, double end, double amount)
		{
			var dif = end - start;
			var adj = dif * amount;

			return start + adj;
		}

		public static float Lerp(this float start, float end, float amount)
		{
			var dif = end - start;
			var adj = dif * amount;

			return start + adj;
		}
	}
}
