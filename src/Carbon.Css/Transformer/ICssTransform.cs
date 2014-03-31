namespace Carbon.Css
{
	using System.Linq;

	public interface ICssTransformer
	{
		void Transform(CssRule rule);
	}

	/*
	public class InlineImports : ICssTransform
	{
		public bool Transform(CssRule rule)
		{
			if (rule.Type != RuleType.Import) return;
		}
	}
	*/

	public class IEOpacityTransform : ICssTransformer
	{
		public void Transform(CssRule rule)
		{
			var declaration = rule.Get("opacity");

			if (declaration == null) return;
			
			var cssValue = declaration.Value;

			int value;

			try { value = (int)(double.Parse(cssValue.ToString()) * 100); }
			catch { return; }

			// Remove any existing filters
			foreach (var filter in rule.FindHavingPropertyName("filter").Where(f => f.Value.ToString().Contains("alpha")).ToArray())
			{
				rule.Remove(filter);
			}

			var index = rule.IndexOf(declaration);

			// Add the filter
			rule.Insert(index, new CssDeclaration("filter", "alpha(opacity=" + value + ")"));
			
		}
	}

	public class AddVendorPrefixesTransform : ICssTransformer
	{
		public void Transform(CssRule rule)
		{
			foreach (var declaration in rule.Where(d => IsPrefixed(d)).ToArray())
			{
				var index = rule.IndexOf(declaration);
				var prop = CssPropertyInfo.Get(declaration.Name.Text);

				var cssValue = declaration.Value;

				foreach (var prefixedName in prop.Compatibility.GetPrefixes(declaration.Name.Text))
				{
					// Remove existing prefixes
					foreach (var remove in rule.FindHavingPropertyName(prefixedName).ToArray())
					{
						rule.Remove(remove);
					}

					// Insert above the rule
					rule.Insert(index, new CssDeclaration(new CssName(prefixedName), cssValue));
				}
			}
		}

		public bool IsPrefixed(CssDeclaration d)
		{
			var prop = CssPropertyInfo.Get(d.Name.Text);

			if (prop == null || prop.Compatibility == null || prop.Compatibility.Prefixed == null) return false;

			return true;
		}
	}
}