namespace Carbon.Css
{
	using System.Linq;


	public class AddVendorPrefixesTransform : ICssTransformer
	{
		private readonly Browser[] targets;

		public AddVendorPrefixesTransform(Browser[] targets)
		{
			this.targets = targets;
		}

		public void Transform(CssRule rule, int ruleIndex)
		{
			foreach (var declaration in rule.Children.OfType<CssDeclaration>().ToArray())
			{
				var prop = CssPropertyInfo.Get(declaration.Name);

				if (!prop.Compatibility.HasPatches) continue;
				
				var index = rule.IndexOf(declaration);
				var cssValue = declaration.Value;

				foreach (var browser in prop.Compatibility.Prefixed.OrderByDescending(b => b.Prefix.Text))
				{
					if (!targets.Any(b => b.Type == browser.Type)) return;

					var fullName = browser.Prefix + prop.Name;

					// Remove existing prefixes
					/*
					foreach (var remove in rule.FindDeclaration(fullName).ToArray())
					{
						rule.Remove(remove);
					}
					*/

					// Clone the value

					// TODO (Select distinct prefixes)
					// Skip webkit for now (to avoid dubs)
					if (browser.Type == BrowserType.Safari) continue;

					var patchedValue = GetPatchedValue(cssValue, browser);

					var patchedDeclaration = new CssDeclaration(fullName, patchedValue);

					// Insert above the rule
					rule.Insert(index, patchedDeclaration);
				}
			}
		}

		public CssValue GetPatchedValue(CssValue value, Browser browser)
		{
			if (value.Kind == NodeKind.ValueList && browser.Prefix.Text == "-webkit-")
			{
				var a = (CssValueList)value;

				var list = new CssValueList(a.Seperator);

				foreach (var node in a.Children)
				{
					if (node.Text == "transform")
					{
						list.Add(new CssString("-webkit-transform"));
					}
					else
					{
						list.Add(node);
					}
				}

				return list;
			}

			else
			{
				return value;
			}
		}
	}
}
