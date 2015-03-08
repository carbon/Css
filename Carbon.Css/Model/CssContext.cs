namespace Carbon.Css
{
	using Carbon.Css.Model;
	using System;
	using System.Collections.Generic;

	public class CssContext
	{
		private readonly Dictionary<string, MixinNode> mixins = new Dictionary<string, MixinNode>();
		private readonly CssScope scope = new CssScope();

		private readonly CssContext parent;

		private int counter = 0;

		public CssFormatting Formatting { get; set; }

		public CssScope Scope
		{
			get { return scope; }
		}

		public Dictionary<string, MixinNode> Mixins
		{
			get { return mixins; }
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
