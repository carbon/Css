namespace Carbon.Css
{
	using System.Collections.Generic;
	using System.Linq;

	public class CssContext
	{
		private readonly Dictionary<string, MixinNode> mixins = new Dictionary<string, MixinNode>();
		private readonly CssScope scope = new CssScope();

		private Browser[] browserSupport = null;

		public CssFormatting Formatting { get; set; }

		public CssScope Scope
		{
			get { return scope; }
		}

		public Dictionary<string, MixinNode> Mixins
		{
			get { return mixins; }
		}

		public CssScope GetNestedScope()
		{
			return new CssScope(scope);
		}

		private bool cs = false;

		public Browser[] BrowserSupport
		{
			get { return browserSupport; }
		}

		public void SetCompatibility(params Browser[] targets)
		{
			if (browserSupport != null) return;

			browserSupport = targets.OrderBy(t => t.Prefix.Text).ToArray();
		}
	}

	public enum CssFormatting
	{
		Original = 1,
		Pretty = 2,
		None = 3
	}
}
