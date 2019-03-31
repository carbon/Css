using Carbon.Color;

namespace Carbon.Css
{
    public sealed class CssColor : CssValue
	{
        private readonly object value; // Rgba32 | string

		public CssColor(string value)
			: base(NodeKind.Color)
		{
			this.value = value;
		}

		public CssColor(Rgba32 value)
			: base(NodeKind.Color)
		{
            this.value = value;
		}

        private CssColor(object value)
            : base(NodeKind.Color)
        {
            this.value = value;
        }

		public override string ToString() => value.ToString();

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