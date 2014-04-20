namespace Carbon.Css
{
	using System;
	using System.Text;

	public class CssDeclaration : CssNode
	{
		private readonly string name;
		private readonly CssValue value;
		private readonly string priority;

		public CssDeclaration(string name, string value, string priority = null)
			: this(name, CssValue.Parse(value), priority) { }

		public CssDeclaration(string name, CssValue value, string priorty = null)
			: base(NodeKind.Declaration)
		{
			#region Preconditions

			if (name == null)	throw new ArgumentNullException("name");
			if (value == null)	throw new ArgumentNullException("value");

			#endregion

			this.name = name;
			this.value = value;
			this.priority = priorty;
		}

		public string Name
		{
			get { return name; }
		}

		public CssValue Value
		{
			get { return value; }
		}

		public CssPropertyInfo Info
		{
			get { return CssPropertyInfo.Get(this.name); }
		}

		public string Priority
		{
			get { return priority; }
		}

		public override string Text
		{
			get { return ToString(); }
		}

		public override string ToString()
		{
			// color: red !important

			var sb = new StringBuilder();

			sb.Append(name).Append(": ").Append(value.ToString());

			if (priority != null)
			{
				sb.Append(" !").Append(priority);
			}

			return sb.ToString();
		}
	}
}
