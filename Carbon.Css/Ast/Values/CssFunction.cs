namespace Carbon.Css
{
	using Carbon.Css.Parser;
	using System.Collections.Generic;

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

		public CssValue Args
		{
			get { return args; }
		}

		public override string Text
		{
			get { return name.Text + "(" + args.Text + ")"; }
		}

		// Add Children to allow recussive variable binding
		public override IList<CssNode> Children
		{
			get { return new[] { Args }; }
		}

		public override string ToString()
		{
			return Text;
		}
	}
}
