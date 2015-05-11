namespace Carbon.Css
{
	using System.Collections.Generic;

	public interface ICssRewriter
	{
		IEnumerable<CssRule> Rewrite(CssRule rule);
	}
}