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

		public List<CssDeclaration> ToAdd
		{
			get { return add; }
		}

		public List<CssDeclaration> ToRemove
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

			var valueText = declaration.Value.ToString();

			if (declaration.Property.Name == "opacity")
			{
				int value = -1;

				try { value = (int)(float.Parse(valueText) * 100); }
				catch { }

				if (value > -1)
				{
					rewrite.ToAdd.Add(new CssDeclaration("filter", string.Format("alpha(opacity={0})", value.ToString())));

					foreach (var filter in rule.Block.FindHavingProperty(CssProperty.Get("filter")).Where(f => f.Value.ToString().Contains("alpha")))
					{
						rewrite.ToRemove.Add(filter);
					}
				}
			}
			else
			{
				foreach (var prefix in declaration.Property.GetPrefixedProperties())
				{
					rewrite.ToAdd.Add(new CssDeclaration(prefix, valueText));

					// Remove existing prefixes
					rewrite.ToRemove.AddRange(rule.Block.FindHavingProperty(prefix));
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

				foreach (var add in rewrite.ToAdd)
				{
					rule.Block.Declarations.Insert(indexOf, add);
				}

				foreach (var remove in rewrite.ToRemove)
				{
					rule.Block.Remove(remove);
				}
			}
		}
	}
}
