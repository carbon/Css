namespace Carbon.Css
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public class SassRewriter : ICssRewriter
	{
		private readonly CssContext context;

		public SassRewriter(CssContext context)
		{
			this.context = context;
		}

		public int Order
		{
			get { return 2; }
		}

		public IEnumerable<CssRule> Rewrite(CssRule rule)
		{
			if (rule.Type != RuleType.Style) yield break;

			if (rule.All(r => r.Kind == NodeKind.Declaration)) yield break;

			var styleRule = (StyleRule)rule;

			// Expand styles if it's a multiselector
			if (styleRule.Selector.Count > 1)
			{
				foreach (var s in styleRule.Selector)
				{
					var children = rule.CloneNode().Children;

					foreach (var a in Rewrite(new StyleRule(s, children)))
					{
						yield return a;
					}
				}

				rule.Children.Clear();

				// TODO: Figure out how to combine back together

				yield break;
			}


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
				var newRules = Expand(
					rule   : nestedRule,
					parent : rule
				);

				foreach (var r in newRules)
				{
					yield return r;
				}

			}
		}


		public IEnumerable<CssRule> Expand(StyleRule rule, CssRule parent)
		{
			var newRule = new StyleRule(GetSelector(rule));

			foreach (var childNode in rule.Children.ToArray())
			{
				if (childNode is StyleRule)
				{
					var childRule = (StyleRule)childNode;

					foreach (var r in Expand(childRule, rule))
					{
						yield return r;
					}
				}
				else
				{
					newRule.Add(childNode);
				}
			}

			parent.Remove(rule); // Remove from parent node after it's been processed

			if (newRule.Children.Count > 0)
			{
				yield return newRule;
			}
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

			foreach (var node in mixin.Children.ToArray())
			{
				// Bind variables

				if (node is IncludeNode)
				{
					ExpandInclude(
						(IncludeNode)node,
						rule
					);

					mixin.Children.Remove(node);
				}

				BindVariables(node, childContext);

				rule.Insert(i + 1, node.CloneNode());

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
