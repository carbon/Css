namespace Carbon.Css
{
	using System;
	using System.Collections.Generic;

	public class CssContext
	{
		private readonly Dictionary<string, CssValue> variables = new Dictionary<string,CssValue>();
		private readonly Dictionary<string, MixinNode> mixins = new Dictionary<string, MixinNode>();

		private readonly CssContext parent;

		private int counter = 0;

		public CssContext() { }

		public CssContext(CssContext parent)
		{
			this.parent = parent;
		}

		public CssFormatting Formatting { get; set; }

		public Dictionary<string, CssValue> Variables
		{
			get { return variables;  }
		}

		public CssValue GetVariable(string name)
		{
			counter++;

			if (counter > 10000) throw new Exception("recussion detected");

			CssValue value;

			if (variables.TryGetValue(name, out value))
			{
				if (value.Kind == NodeKind.Variable)
				{
					var variable = (CssVariable)value;

					if (variable.Symbol == name) throw new Exception("Self referencing");

					return GetVariable(variable.Symbol);
				}
				
				return value;
			}

			if (parent != null && parent.Variables.TryGetValue(name, out value))
			{
				if (value.Kind == NodeKind.Variable)
				{

					var variable = (CssVariable)value;

					if (variable.Symbol == name) throw new Exception("Self referencing");

					return parent.GetVariable(variable.Symbol);
				}
				
				return value;
				
			}


			return new CssString(string.Format("/* ${0} not found */", name));
		}

		public Dictionary<string, MixinNode> Mixins
		{
			get { return mixins; }
		}

		public CssContext Parent
		{
			get { return parent; }
		}

		#region Rewriters

		private readonly RewriterCollection rewriters = new RewriterCollection();

		private bool cs = false;

		public void SetCompatibility(params Browser[] targets)
		{
			if (cs) return;

			cs = true;

			rewriters.Add(new AddPrefixes(targets));
		}

		public void AllowNestedRules()
		{
		}

		public void AddRewriter(ICssRewriter rewriter)
		{
			rewriters.Add(rewriter);
		}

		public RewriterCollection Rewriters
		{
			get { return rewriters; }
		}

		#endregion
	}

	public enum CssFormatting
	{
		Original = 1,
		Pretty = 2,
		None = 3
	}
}
