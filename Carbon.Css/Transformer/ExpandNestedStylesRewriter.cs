namespace Carbon.Css
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public class ExpandNestedStylesRewriter : ICssTransformer
	{
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
			foreach (var nestedRule in rule.Children.OfType<CssRule>().ToArray())
			{
				Expand(
					nested		: nestedRule,
					parent		: rule,
					index		: ref index,
					styleSheet	: styleSheet
				);
			}
		}


		public void Expand(CssRule nested, CssRule parent, StyleSheet styleSheet, ref int index)
		{
			var newRule = new CssRule(
				type		: RuleType.Style,
				selector	: GetSelector(nested)
			);

			foreach (var childNode in nested.Children.ToArray())
			{
				if (childNode is CssRule)
				{
					var childRule = (CssRule)childNode;

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

		public CssSelector GetSelector(CssRule nested)
		{
			var parts = new List<string>();

			var selector = nested.Selector.ToString();

			parts.Add(selector);

			CssRule current = nested;

			while ((current = current.Parent as CssRule) != null)
			{
				parts.Add(current.Selector.ToString());

				if (parts.Count > 5) throw new Exception(string.Format("Cannot nest more than 5 levels deep. Was {0}. ", string.Join(" ", parts)));
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
				if (i != parts.Count)
				{
					sb.Append(' ');
				}

				sb.Append(part);
			}

			return new CssSelector(sb.ToString());
		}
	}
}
