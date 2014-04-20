namespace Carbon.Css
{
	public class CssParameter
	{
		private readonly string name;
		private readonly CssValue @default;

		public CssParameter(string name, CssValue @default = null)
		{
			this.name = name;
			this.@default = @default;
		}
		
		public string Name 
		{
			get { return name; }
		}

		public CssValue Default
		{
			get { return @default; }
		}
	}
}
