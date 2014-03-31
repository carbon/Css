namespace Carbon.Css
{
	public class CssContext
	{
		private readonly VariableBag variables = new VariableBag();

		public CssFormatting Formatting { get; set; }

		public VariableBag Variables
		{
			get { return variables;  }
		}
	}

	public enum CssFormatting
	{
		Original = 1,
		Pretty = 2,
		None = 3
	}
}
