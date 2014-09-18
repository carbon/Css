namespace Carbon.Css
{
	public class CssColor : CssValue
	{
		private readonly string value;

		public CssColor(string value)
			: base(NodeKind.Color)
		{
			this.value = value;
		}

		public override string Text
		{
			get { return value; }
		}

		public override string ToString()
		{
			return value;
		}

		public override CssNode Clone()
		{
			return new CssColor(value);
		}
	}
}

// hsl
// rgba
// hsla
// #hex
// named (purple, yellow, orange)