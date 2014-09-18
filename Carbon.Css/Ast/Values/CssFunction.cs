namespace Carbon.Css
{
	using Carbon.Css.Parser;
	using System.Collections.Generic;

	public class CssFunction : CssValue
	{
		private string name;
		private CssValue args;

		public CssFunction(string name, CssValue args)
			: base(NodeKind.Function)
		{
			this.name = name;
			this.args = args;
		}

		public string Name
		{
			get { return name; }
		}

		public CssValue Args
		{
			get { return args; }
		}

		public override string Text
		{
			get { return name + "(" + args.Text + ")"; }
		}

		// Add Children to allow recussive variable binding
		public override IList<CssNode> Children
		{
			get { return new[] { Args }; }
		}

		public override CssNode CloneNode()
		{
			return new CssFunction(name, args);
		}

		public override string ToString()
		{
			return Text;
		}
	}
}
