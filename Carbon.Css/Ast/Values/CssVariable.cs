namespace Carbon.Css
{
	using Parser;

	public class CssVariable : CssValue
	{
		private readonly string text;

		public CssVariable(CssToken token)
			: this(token.Text)
		{ }

		public CssVariable(string text)
			: base(NodeKind.Variable) 
		{ 
			this.text = text;
		}

		public string Symbol => text;

		// When bound
		public CssValue Value { get; set; }

		public override CssNode CloneNode() => new CssVariable(text) { Value = Value };

		public override string ToString() => Value?.ToString() ?? "[null]";
	}
}

// Variable (mathematics), a symbol that represents a quantity in a mathematical expression