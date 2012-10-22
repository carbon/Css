namespace Carbon.Css
{
	using System;
	using System.Collections.Generic;

	public class CssDeclaration
	{
		private readonly CssProperty property;
		private readonly CssValue value;

		public CssDeclaration(string propertyName, string valueText)
			: this(CssProperty.Get(propertyName), CssValue.Parse(valueText)) { }

		public CssDeclaration(CssProperty property, CssValue value)
		{
			#region Preconditions

			if (property == null) 
				throw new ArgumentNullException("property");

			#endregion

			this.property = property;
			this.value = value;
		}

		public CssProperty Property
		{
			get { return property; }
		}

		public CssValue Value
		{
			get { return value; }
		}

		public static CssDeclaration Parse(string text)
		{
			#region

			if (text == null) throw new ArgumentNullException("text");

			#endregion

			// property : value

			text = text.Replace('\t', ' ').Replace('\n', ' ').Replace('\r', ' ');

			var colonIndex = text.IndexOf(':');

			var propertyName = text.Substring(0, colonIndex).Trim();
			var valueText = text.Substring(colonIndex + 1);

			return new CssDeclaration(propertyName, valueText);
		}
	}
}
