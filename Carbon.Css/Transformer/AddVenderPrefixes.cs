namespace Carbon.Css
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	// Directive
	public class AddPrefixes : ICssTransformer
	{
		private readonly Browser[] targets;

		public AddPrefixes(IList<Browser> targets)
		{
			this.targets = targets.OrderByDescending(t => t.Prefix.Text).ToArray(); // Reverse the list
		}

		public int Order
		{
			get { return 2; }
		}

		public void Transform(CssRule rule, int ruleIndex)
		{
			if (rule.Type == RuleType.Keyframes)
			{
				var keyframes = (KeyframesRule)rule;

				// TODO: Add -webkit & moz versions

				if (targets.Any(a => a.Type == BrowserType.Firefox && a.Version < 16))
				{
					var index = rule.Parent.Children.IndexOf(rule);

					var newRule = GetPrefixedKeyframeRule(keyframes, Browser.Firefox1);

					rule.Parent.Children.Insert(index, newRule);
				}


				// -webkit
				if (targets.Any(a => a.Type == BrowserType.Safari))
				{
					var index = rule.Parent.Children.IndexOf(rule);

					var newRule = GetPrefixedKeyframeRule(keyframes, Browser.Safari1);

					rule.Parent.Children.Insert(index, newRule);
				}

				return;
			}

			Expand(rule);
		}

		public AtRule GetPrefixedKeyframeRule(KeyframesRule rule, Browser browser)
		{
			var newRule = new AtRule(browser.Prefix + "keyframes", RuleType.Unknown, rule.Name);

			foreach (var child in rule.OfType<StyleRule>())
			{
				var a = new StyleRule(child.Selector.ToString());

				foreach (var c2 in child.OfType<CssDeclaration>())
				{
					var prop = CssPropertyInfo.Get(c2.Name);


					var name = (prop.Compatibility.HasPatches) ? browser.Prefix + c2.Name : c2.Name;

					a.Add(new CssDeclaration(name, GetPatchedValueFor(c2.Value, browser)));
				}

				newRule.Add(a);
			}


			return newRule;
		}

		public void Expand(CssRule rule)
		{
			foreach (var declaration in rule.Children.OfType<CssDeclaration>().ToList())
			{
				var prop = CssPropertyInfo.Get(declaration.Name);

				if (!prop.Compatibility.HasPatches) continue;

				var index = rule.IndexOf(declaration);
				var cssValue = declaration.Value;

				var prefixes = BrowserPrefixKind.None;

				foreach (var browser in targets) // Iterated backwards
				{
					if (!prop.Compatibility.IsPrefixed(browser)) continue;

					// Skip the prefix if we've already added it
					if (prefixes.HasFlag(browser.Prefix.Kind)) continue;

					var fullName = browser.Prefix + prop.Name;

					/*
					// TODO: Make sure we don't add a prefix if it already exists
					foreach (var remove in rule.FindDeclaration(fullName).ToArray())
					{
						rule.Remove(remove);
					}
					*/

					var patchedValue = (prop.Compatibility.HasValuePatches)
						? GetPatchedValueFor(cssValue, browser)
						: cssValue;

					var patchedDeclaration = new CssDeclaration(fullName, patchedValue);

					// Insert above the rule
					rule.Insert(index, patchedDeclaration);

					prefixes |= browser.Prefix.Kind;
				}
			}
		}

		// transform 0.04s linear, opacity 0.04s linear, visibility 0.04s linear;

		public CssValue GetPatchedValueFor(CssValue value, Browser browser)
		{
			if (value.Kind != NodeKind.ValueList) return value;

			var a = (CssValueList)value;

			var list = new CssValueList(a.Seperator);

			foreach (var node in a.Children)
			{
				if (node.Kind == NodeKind.ValueList) // For comma seperated componented lists
				{
					list.Add(GetPatchedValueFor((CssValue)node, browser));
				}
				else if (node.Kind == NodeKind.String && node.Text == "transform")
				{
					list.Add(new CssString(browser.Prefix.Text + "transform"));
				}
				else
				{
					list.Add(node);
				}
			}

			return list;
			
		}
	}
}
