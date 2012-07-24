namespace Carbon.Css
{
	using System.Collections.Generic;

	public class CssDeclaration
	{
		private readonly CssProperty property;
		private readonly CssValue value;

		public CssDeclaration(CssProperty property, string valueText)
		{
			this.property = property;
			this.value = CssValue.Parse(valueText);
		}

		public CssDeclaration(string propertyName, string valueText)
		{
			this.property = CssProperty.Get(propertyName);
			this.value = CssValue.Parse(valueText);
		}

		public CssDeclaration(string text)
		{
			// property : value

			text = text.Replace('\t', ' ').Replace('\n', ' ').Replace('\r', ' ');

			var indexOfColon = text.IndexOf(':');

			var propertyName = text.Substring(0, indexOfColon).Trim();
			var valueText = text.Substring(indexOfColon + 1);

			this.property = CssProperty.Get(propertyName);
			this.value = CssValue.Parse(valueText);
		}

		public CssProperty Property
		{
			get { return property; }
		}

		public CssValue Value
		{
			get { return value; }
		}
	}
}
