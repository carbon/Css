namespace Carbon.Css
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	public class IEOpacityTransform : ICssRewriter
	{
		public int Order
		{
			get { return 3; }
		}

		public IEnumerable<CssRule> Rewrite(CssRule rule)
		{
			var declaration = rule.Get("opacity");

			if (declaration == null) yield break;

			var value = declaration.Value as CssNumber;

			if (value == null) yield break;

			// Remove any existing filters
			foreach (var filter in rule.FindDeclaration("filter").Where(f => f.Value.ToString().StartsWith("alpha")).ToArray())
			{
				rule.Remove(filter);
			}

			var index = rule.IndexOf(declaration);

			// Add the filter
			rule.Insert(index, new CssDeclaration("filter", "alpha(opacity=" + value + ")"));

			yield break;
		}
	}
}
