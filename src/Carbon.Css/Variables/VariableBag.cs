namespace Carbon.Css
{
	using System.Collections.Generic;

	public class VariableBag
	{
		private readonly IDictionary<string, string> items = new Dictionary<string, string>();

		public string Get(string name)
		{
			string value;
			
			items.TryGetValue(name, out value);

			return value;
		}

		public void Set(string name, string value)
		{
			items[name] = value;
		}
	}
}
