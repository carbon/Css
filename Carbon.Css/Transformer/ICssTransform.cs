namespace Carbon.Css
{
	using System.Linq;

	public interface ICssTransformer // Rename ICssRewriter
	{
		void Transform(CssRule rule, int index);

		int Order { get; }
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