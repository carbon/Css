using System.Collections.Generic;
namespace Carbon.Css
{
	public class CssContext
	{
		private readonly VariableBag variables = new VariableBag();
		private readonly Dictionary<string, MixinNode> mixins = new Dictionary<string, MixinNode>();

		private readonly CssContext parent;

		public CssContext() { }

		public CssContext(CssContext parent)
		{
			this.parent = parent;
		}

		public CssFormatting Formatting { get; set; }

		public VariableBag Variables
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

			if (parent != null)
			{
				variables.TryGetValue(name, out value);
			}

			return value;
		}

		public Dictionary<string, MixinNode> Mixins
		{
			get { return mixins; }
		}

		public CssContext Parent
		{
			get { return parent; }
		}
	}

	public enum CssFormatting
	{
		Original = 1,
		Pretty = 2,
		None = 3
	}
}
