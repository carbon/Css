namespace Carbon.Css
{
	using Carbon.Css.Parser;

	public class CssFunction : CssValue
	{
		private CssToken name;
		private CssValue args;

		public CssFunction(CssToken name, CssValue args)
			: base(NodeKind.Function)
		{
			this.name = name;
			this.args = args;
		}

		public string Name
		{
			get { return name.Text; }
		}

		public override string Text
		{
			get { return name.Text + "(" + args.Text + ")"; }
		}

		public override string ToString()
		{
			return Text;
		}
	}
}
