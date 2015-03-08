using Carbon.Css.Color;
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

		public CssColor(WebColor value)
			: base(NodeKind.Color)
		{
			this.value = value.ToString();
		}

		public override string Text
		{
			get { return value; }
		}

		public override string ToString()
		{
			return value;
		}

		public static CssColor FromRgba(byte r, byte g, byte b, float a)
		{
			return new CssColor(string.Format("rgba({0}, {1}, {2}, {3})", r, g, b, a));
		}

		public override CssNode CloneNode()
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