using System;
using System.Collections.Generic;
namespace Carbon.Css
{
	public class MixinNode : CssRule
	{
		private readonly string name;
		private readonly IList<CssParameter> parameters;

		public MixinNode(string name, IList<CssParameter> parameters, IList<CssDeclaration> declarations)
			: base(RuleType.Mixin, NodeKind.Mixin)
		{
			this.name = name;
			this.parameters = parameters;

			this.declarations.AddRange(declarations);
		}

		public string Name
		{
			get { return name; }
		}

		public IList<CssParameter> Parameters
		{
			get { return parameters; }
		}

		public IList<CssDeclaration> Declarations
		{
			get { return declarations; }
		}
		

		public IEnumerable<CssDeclaration> Execute(CssValue args, CssContext context)
		{
			// TODO: set the variable names based on position

			var list = args.ToList(); // Break only if comma seperated

			var child = new CssContext(context);

			var i = 0;

			foreach (var p in parameters)
			{
				var val = (list.Count >= i + 1) ? list[i] : p.Default;

				child.Variables.Add(p.Name, val);
			
				i++;
			}

			foreach (var declaration in declarations)
			{
				yield return declaration;
			}
		}

		public override string Text
		{
			get { return ""; }
		}

		public override void WriteTo(System.IO.TextWriter writer, int level = 0, CssContext context = null)
		{
			// 
		}
	}
}

/*
@mixin left($dist) {
  float: left;
  margin-left: $dist;
}
*/