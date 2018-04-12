namespace Carbon.Css
{
	using Color;

	public sealed class CssColor : CssValue
	{
		private readonly string value;

		public CssColor(string value)
			: base(NodeKind.Color)
		{
			this.value = value;
		}

		public CssColor(Rgba value)
			: base(NodeKind.Color)
		{
			this.value = value.ToString();
		}

		public override string ToString() => value;

		public static CssColor FromRgba(byte r, byte g, byte b, float a)
		{
			return new CssColor($"rgba({r}, {g}, {b}, {a})");
		}

		public override CssNode CloneNode() => new CssColor(value);
    }
}

// hsl
// rgba
// hsla
// #hex
// named (purple, yellow, orange)