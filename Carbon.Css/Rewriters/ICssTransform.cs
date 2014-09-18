namespace Carbon.Css
{
	using System.Collections.Generic;
	using System.Linq;

	public interface ICssRewriter
	{
		IEnumerable<CssRule> Rewrite(CssRule rule);

		int Order { get; }
	}
}