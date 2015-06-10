using System.Collections.Generic;

namespace Carbon.Css
{
	public class MixinNode : CssBlock
	{
		private readonly string name;
		private readonly IList<CssParameter> parameters;

		public MixinNode(string name, IList<CssParameter> parameters)
			: base(NodeKind.Mixin)
		{
			this.name = name;
			this.parameters = parameters;
		}

		public string Name => name;

		public IList<CssParameter> Parameters => parameters;
	}
}

/*
@mixin left($dist) {
  float: left;
  margin-left: $dist;
}
*/