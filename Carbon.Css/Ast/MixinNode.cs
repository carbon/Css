namespace Carbon.Css
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public class MixinNode : CssBlock
	{
		private readonly string name;
		private readonly IList<CssParameter> parameters;

		public MixinNode(string name, IList<CssParameter> parameters, List<CssNode> children)
			: base(NodeKind.Mixin, children)
		{
			this.name = name;
			this.parameters = parameters;
		}

		public string Name
		{
			get { return name; }
		}

		public IList<CssParameter> Parameters
		{
			get { return parameters; }
		}

		public override string Text
		{
			get { return ""; }
		}
	}
}

/*
@mixin left($dist) {
  float: left;
  margin-left: $dist;
}
*/