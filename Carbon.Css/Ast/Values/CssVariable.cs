namespace Carbon.Css
{
	using Carbon.Css.Parser;

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

		public string Symbol
		{
			get { return text; }
		}

		public override string Text
		{
			get { return text; }
		}

		// When bound
		public CssValue Value { get; set; }


		public override CssNode CloneNode()
		{
			return new CssVariable(text) { Value = Value };
		}

		public override string ToString()
		{
			if (Value != null) return Value.ToString();

			return "[null]";
		}
	}
}

// Variable (mathematics), a symbol that represents a quantity in a mathematical expression