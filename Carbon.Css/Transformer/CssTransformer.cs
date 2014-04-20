namespace Carbon.Css
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using System.Text;

	public class DefaultRuleTransformer : ICssTransformer
	{
		private readonly List<ICssTransformer> transformers = new List<ICssTransformer> {
			new IEOpacityTransform(),
			new AddVendorPrefixesTransform()
		};

		public void Transform(CssRule rule)
		{
			foreach (var transformer in transformers)
			{
				transformer.Transform(rule);				
			}
		}
	}
}
