namespace Carbon.Css
{
	using System.Collections.Generic;
	using System.Linq;

	public class CssContext
	{
		private readonly Dictionary<string, CssValue> variables = new Dictionary<string,CssValue>();
		private readonly Dictionary<string, MixinNode> mixins = new Dictionary<string, MixinNode>();

		private readonly CssContext parent;

		public CssContext() { }

		public CssContext(CssContext parent)
		{
			this.parent = parent;
		}

		public CssFormatting Formatting { get; set; }


		public int IncludeCount { get; set; }

		public Dictionary<string, CssValue> Variables
		{
			get { return variables;  }
		}

		public CssValue GetVariable(string name)
		{
			CssValue value;

			if (variables.TryGetValue(name, out value))
			{
				return value;
			}

			if (parent != null && parent.Variables.TryGetValue(name, out value))
			{
				return value;
			}

			return new CssString("");
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
			rewriters.Add(new SassRewriter(this));
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
