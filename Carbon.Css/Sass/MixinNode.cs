namespace Carbon.Css
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public class MixinNode : CssRule
	{
		private readonly string name;
		private readonly IList<CssParameter> parameters;

		public MixinNode(string name, IList<CssParameter> parameters, IList<CssNode> children)
			: base(RuleType.Mixin, NodeKind.Mixin)
		{
			this.name = name;
			this.parameters = parameters;

			foreach (var child in children)
			{
				this.children.Add(child);
			}
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