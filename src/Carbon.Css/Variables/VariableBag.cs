namespace Carbon.Css
{
	using System.Collections.Generic;

	public class VariableBag
	{
		private readonly IDictionary<string, CssValue> items = new Dictionary<string, CssValue>();

		public CssValue Get(string name)
		{
			CssValue value;
			
			items.TryGetValue(name, out value);

			return value;
		}

		public void Set(string name, CssValue value)
		{
			items[name] = value;
		}
	}
}
