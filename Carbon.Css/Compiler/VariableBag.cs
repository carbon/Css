namespace Carbon.Css
{
	using System;
	using System.Collections.Generic;

	public class VariableBag : Dictionary<string, CssValue>
	{

		public CssValue Get(string name)
		{
			CssValue value;

			if (!TryGetValue(name, out value))
			{
				throw new Exception(string.Format("'{0}' not found in variables", name));
			}

			return value;
		}

		
	}
}
