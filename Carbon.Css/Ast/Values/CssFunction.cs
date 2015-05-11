using System.Collections.Generic;

namespace Carbon.Css
{
	public class CssFunction : CssValue
	{
		private readonly string name;
		private readonly CssValue args;

		public CssFunction(string name, CssValue args)
			: base(NodeKind.Function)
		{
			this.name = name;
			this.args = args;
		}

		public string Name => name;

		public CssValue Args => args;

		// Add Children to allow recussive variable binding
		public override IList<CssNode> Children => new[] { Args };

		public override CssNode CloneNode() => new CssFunction(name, args);

		public override string ToString() => name + "(" + args.ToString() + ")";
	}
}
