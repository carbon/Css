namespace Carbon.Css
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public class ExpandNestedStylesRewriter : ICssTransformer
	{
		private readonly CssContext context;

		public ExpandNestedStylesRewriter(CssContext context)
		{
			this.context = context;
		}

		public int Order
		{
			get { return 2; }
		}

		public void Transform(CssRule rule, int index)
		{
			if (rule.Type != RuleType.Style) return;

			var styleSheet = rule.Parent as StyleSheet;

			if (styleSheet == null) return;

			Expand(rule, index, styleSheet);

			// Remove the rule if we've moved all it's children up
			if (rule.Children.Count == 0)
			{
				styleSheet.Children.Remove(rule);
			}
			
		}

		public void Expand(CssRule rule, int index, StyleSheet styleSheet)
		{
			if (rule.All(r => r.Kind == NodeKind.Declaration)) return;

			foreach (var includeNode in rule.Children.OfType<IncludeNode>().ToArray())
			{
				ExpandInclude(
					includeNode,
					rule
				);

				rule.Children.Remove(includeNode);
			}

			foreach (var nestedRule in rule.Children.OfType<StyleRule>().ToArray())
			{
				Expand(
					nested		: nestedRule,
					parent		: rule,
					index		: ref index,
					styleSheet	: styleSheet
				);
			}
		}


		public void Expand(StyleRule nested, CssRule parent, StyleSheet styleSheet, ref int index)
		{
			var newRule = new StyleRule(GetSelector(nested));

			foreach (var childNode in nested.Children.ToArray())
			{
				if (childNode is StyleRule)
				{
					var childRule = (StyleRule)childNode;

					Expand(childRule, nested, styleSheet, ref index);
				}
				else
				{
					newRule.Add(childNode);
				}
			}

			// Remove from parent node after it's been processed
			parent.Remove(nested);

			if (newRule.Children.Count > 0)
			{
				index++;

				styleSheet.Children.Insert(index, newRule);
			}

			return;
		}

		public CssSelector GetSelector(StyleRule nested)
		{
			var parts = new List<string>();

			var selector = nested.Selector.ToString();

			parts.Add(selector);

			StyleRule current = nested;

			while ((current = current.Parent as StyleRule) != null)
			{
				parts.Add(current.Selector.ToString());

				if (parts.Count > 6)
				{
					throw new Exception(string.Format("Cannot nest more than 6 levels deep. Was {0}. ", string.Join(" ", parts)));
				}
			}

			var sb = new StringBuilder();

			for (var i = parts.Count; --i >= 0; )
			{
				var part = parts[i];
				
				if (part.Contains('&'))
				{
					part = part.Replace("&", sb.ToString());

					sb.Clear();
				}

				if (i != parts.Count) sb.Append(' ');

				// h1, h2, h3

				var split = part.Split(',');

				if (split.Length > 1)
				{
					var parentSelector = sb.ToString();

					sb.Append(split[0].Trim());

					foreach(var a in split.Skip(1))
					{
						sb.Append(", " + parentSelector + a.Trim());
					}
				}
				else
				{
					sb.Append(part);
				}
			}

			return new CssSelector(sb.ToString().Trim());
		}


		#region Includes

		public void ExpandInclude(IncludeNode include, CssBlock rule)
		{
			context.IncludeCount++;

			if (context.IncludeCount > 1000) throw new Exception("Exceded include count of 1,000");

			MixinNode mixin;

			if (!context.Mixins.TryGetValue(include.Name, out mixin))
			{
				throw new Exception(string.Format("Mixin '{0}' not registered", include.Name));
			}

			var index = rule.Children.IndexOf(include);

			var childContext = GetContext(mixin.Parameters, include.Args);

			var i = 0;

			var ss = (StyleSheet)rule.Parent;

			foreach (var node in mixin.Children)
			{
				// Bind variables

				if (node is IncludeNode) continue;

				BindVariables(node, childContext);

				rule.Insert(i + 1, node.Clone());

				i++;
			}

		}

		public void BindVariables(CssNode node, CssContext c)
		{
			if (node.Kind == NodeKind.Declaration)
			{
				var declaration = (CssDeclaration)node;

				// TODO: Remove
				// throw new Exception(declaration.Value.Kind.ToString() + ":" + declaration.Value.ToString());

				BindVariables(declaration.Value, c);
			}
			else if (node.Kind == NodeKind.Variable)
			{
				var variable = (CssVariable)node;

				variable.Value = c.GetVariable(variable.Symbol);

			}
			else if (node.HasChildren)
			{
				foreach (var n in node.Children)
				{
					BindVariables(n, c);
				}
			}
		}

		public CssContext GetContext(IList<CssParameter> paramaters, CssValue args)
		{
			var list = new List<CssValue>();

			if (args != null)
			{
				var valueList = args as CssValueList;

				if (valueList == null)
				{
					list.Add(args); // Single Value
				}

				if (valueList != null && valueList.Seperator == ValueListSeperator.Comma)
				{
					list.AddRange(valueList.Children.OfType<CssValue>());
				}
			}

			var child = new CssContext(context);

			var i = 0;

			foreach (var p in paramaters)
			{
				var val = (list != null && list.Count >= i + 1) ? list[i] : p.Default;

				child.Variables.Add(p.Name, val);

				i++;
			}

			return child;
		}

		#endregion
	}
}
