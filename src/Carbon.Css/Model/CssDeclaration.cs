namespace Carbon.Css
{
	using System;
	using System.Collections.Generic;

	public class CssDeclaration
	{
		private readonly string name;
		private readonly CssValue value;

		public CssDeclaration(string name, string value)
			: this(name, CssValue.Parse(value)) { }

		public CssDeclaration(string name, CssValue value)
		{
			#region Preconditions

			if (name == null)	throw new ArgumentNullException("name");
			if (value == null)	throw new ArgumentNullException("value");

			#endregion

			this.name = name;
			this.value = value;
		}

		public string Name
		{
			get { return name; }
		}

		public CssValue Value
		{
			get { return value; }
		}

		public CssPropertyInfo GetPropertyInfo()
		{
			return CssPropertyInfo.Get(name);
		}

		public override string ToString()
		{
			// color: red
			return name + ": " + value.ToString();
		}
	}
}
