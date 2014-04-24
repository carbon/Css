namespace Carbon.Css
{
	using System.Linq;

	public interface ICssTransformer // Rename ICssRewriter
	{
		void Transform(CssRule rule, int index);
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

	
}