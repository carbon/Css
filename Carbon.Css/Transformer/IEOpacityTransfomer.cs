namespace Carbon.Css
{
	using System.Linq;

	public class IEOpacityTransform : ICssTransformer
	{
		public void Transform(CssRule rule, int ruleIndex)
		{
			var declaration = rule.Get("opacity");

			if (declaration == null) return;

			var value = declaration.Value as CssNumber;

			if (value == null) return;

			// Remove any existing filters
			foreach (var filter in rule.FindDeclaration("filter").Where(f => f.Value.ToString().StartsWith("alpha")).ToArray())
			{
				rule.Remove(filter);
			}

			var index = rule.IndexOf(declaration);

			// Add the filter
			rule.Insert(index, new CssDeclaration("filter", "alpha(opacity=" + value + ")"));
		}
	}
}
