using System;

namespace Carbon.Css
{
	public class IncludeNode : CssNode
	{
		private readonly string name;
		private readonly CssValue args;

		public IncludeNode(string name, CssValue args)
			: base(NodeKind.Include)
		{
			#region Preconditions

			if (name == null) throw new ArgumentNullException(nameof(name));

			#endregion

			this.name = name;
			this.args = args;
		}

		public string Name => name;
		
		public CssValue Args => args;

		public override CssNode CloneNode() => new IncludeNode(name, args);
	}
}

// @include box-emboss(0.8, 0.05);