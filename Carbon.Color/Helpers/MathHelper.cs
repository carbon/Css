namespace Carbon.Color
{
	internal static class MathHelper
	{
		public static float Lerp(this float start, float end, float amount)
		{
			var dif = end - start;
			var adj = dif * amount;

			return start + adj;
		}
	}
}
