namespace Carbon.Css
{
	public class CssContext
	{
		private readonly VariableBag variables = new VariableBag();

		public VariableBag Variables
		{
			get { return variables;  }
		}
	}
}
