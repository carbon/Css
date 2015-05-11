namespace Carbon.Css
{
	public class CssParameter
	{
		private readonly string name;
		private readonly CssValue defaultValue;

		public CssParameter(string name, CssValue defaultValue = null)
		{
			this.name = name;
			this.defaultValue = defaultValue;
		}
		
		public string Name => name;

		public CssValue DefaultValue => defaultValue;
	}
}