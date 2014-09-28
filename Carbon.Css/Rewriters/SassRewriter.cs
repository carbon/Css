namespace Carbon.Css
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public class SassRewriter : ICssRewriter
	{
		private readonly CssContext context;
		private readonly List<CssRule> root = new List<CssRule>();

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
			var styleRule = rule as StyleRule;

			if (styleRule == null || rule.All(r => r.Kind == NodeKind.Declaration))
			{
				yield return rule;

				yield break;
			}

			var clone = (StyleRule)rule.CloneNode();

			root.Clear();

			root.Add(clone);

			// Expand includes
			foreach (var includeNode in clone.Children.OfType<IncludeNode>().ToArray())
			{
				ExpandInclude(
					includeNode,
					clone
				);

				clone.Children.Remove(includeNode);
			}

			foreach (var nestedRule in clone.Children.OfType<StyleRule>().ToArray())
			{
				foreach (var r in Expand(
					rule: nestedRule,
					parent: clone
				))
				{
					root.Add(r);
				}
			}

			foreach (var r in root)
			{
				if (!r.Childless) yield return r;
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

		public CssSelector GetSelector(StyleRule rule)
		{
			var parts = new Stack<StyleRule>();

			parts.Push(rule);

			StyleRule current = rule;

			while ((current = current.Parent as StyleRule) != null)
			{
				parts.Push(current);

				if (parts.Count > 6)
				{
					throw new Exception(string.Format("Cannot nest more than 6 levels deep. Was {0}. ", string.Join(" ", parts)));
				}
			}

			var i = 0;

			var sb = new StringBuilder();


			foreach (var part in parts)
			{
				if (part.Selector.Contains("&"))
				{
					var x = part.Selector.ToString().Replace("&", sb.ToString());

					sb.Clear();

					sb.Append(x);

					i++;

					continue;
				}

				if (i != 0) sb.Append(' ');

				i++;

				// h1, h2, h3

				if (part.Selector.Count > 1)
				{
					var parentSelector = sb.ToString();

					sb.Clear();

					var c = GetSelector(parts.Skip(i));

					var q = 0;

					foreach (var a in part.Selector)
					{
						if (q != 0) sb.Append(", ");

						sb.Append(parentSelector + a);

						if (c != null)
						{
							sb.Append(" " + c);
						}

						q++;	
					}

					break;
				}
				else
				{
					sb.Append(part.Selector);
				}
			}
			
			return new CssSelector(sb.ToString());
			
		}

		private string GetSelector(IEnumerable<StyleRule> rules)
		{
			// TODO: & support

			var i = 0;

			var sb = new StringBuilder();

			foreach (var rule in rules)
			{
				if (i != 0) sb.Append(' ');

				sb.Append(rule.Selector);
			}

			return sb.ToString();

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
