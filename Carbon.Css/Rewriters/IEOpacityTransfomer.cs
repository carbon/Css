using System.Collections.Generic;

namespace Carbon.Css
{
	public class IEOpacityTransform : ICssRewriter
	{
		public IEnumerable<CssRule> Rewrite(CssRule rule)
		{
			var declaration = rule.GetDeclaration("opacity");

			if (declaration == null)
			{
				yield return rule;

				yield break;
			}

			var value = declaration.Value as CssNumber;

			if (value == null)
			{
				yield return rule;

				yield break;
			}

			var index = rule.IndexOf(declaration);

			// Add the filter before the standard
			rule.Insert(index, new CssDeclaration("filter", $"alpha(opacity={value})"));

			yield return rule;
		}
	}
}