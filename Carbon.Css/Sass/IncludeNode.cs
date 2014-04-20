namespace Carbon.Css
{
	using System;

	public class IncludeNode : CssDeclaration
	{
		private readonly string name;
		private readonly CssValue args;

		public IncludeNode(string name, CssValue args)
			: base(name, args, NodeKind.Include)
		{
			#region Preconditions

			if (name == null) throw new ArgumentNullException("name");

			#endregion

			this.name = name;
			this.args = args;
		}

		public new string Name
		{
			get { return name; }
		}

		public CssValue Args
		{
			get { return args; }
		}
	}
}

// TODO: Don't inhert from Css Delaration

// @include box-emboss(0.8, 0.05);