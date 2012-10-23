namespace Carbon.Css
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using System.Text;

	public class NodeRewrite
	{
		private readonly List<CssDeclaration> add = new List<CssDeclaration>();
		private readonly List<CssDeclaration> remove = new List<CssDeclaration>();

		public int Index { get; set; }

		public CssDeclaration Source { get; set; }

		public List<CssDeclaration> AddList
		{
			get { return add; }
		}

		public List<CssDeclaration> RemoveList
		{
			get { return remove; }
		}
	}

	public class DefaultRuleTransformer
	{
		public NodeRewrite Rewrite(CssRule rule, CssDeclaration declaration)
		{
			var rewrite = new NodeRewrite();

			rewrite.Source = declaration;

			rewrite.Index = rule.Block.IndexOf(declaration);

			var cssValue = declaration.Value;

			if (declaration.Name == "opacity")
			{
				int value = -1;

				try { value = (int)(float.Parse(cssValue.ToString()) * 100); }
				catch { }

				if (value > -1)
				{
					rewrite.AddList.Add(new CssDeclaration("filter", string.Format("alpha(opacity={0})", value.ToString())));

					foreach (var filter in rule.Block.FindHavingProperty(CssPropertyInfo.Get("filter")).Where(f => f.Value.ToString().Contains("alpha")))
					{
						rewrite.RemoveList.Add(filter);
					}
				}
			}
			else
			{
				foreach (var prefix in declaration.GetPropertyInfo().GetPrefixedProperties())
				{
					rewrite.AddList.Add(new CssDeclaration(prefix.Name, cssValue));

					// Remove existing prefixes
					rewrite.RemoveList.AddRange(rule.Block.FindHavingProperty(prefix));
				}
			}

			return rewrite;
		}

		public void Transform(CssRule rule)
		{
			var rewrites = new List<NodeRewrite>();

			// Padding padding = null;

			foreach (var declaration in rule.Block)
			{
				rewrites.Add(Rewrite(rule, declaration));
			}

			foreach (var rewrite in rewrites)
			{
				var indexOf = rule.Block.Declarations.IndexOf(rewrite.Source);

				foreach (var add in rewrite.AddList)
				{
					rule.Block.Declarations.Insert(indexOf, add);
				}

				foreach (var remove in rewrite.RemoveList)
				{
					rule.Block.Remove(remove);
				}
			}
		}
	}
}
