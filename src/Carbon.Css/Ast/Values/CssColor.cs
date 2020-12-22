using System.IO;

using Carbon.Color;

namespace Carbon.Css
{
    public sealed class CssColor : CssValue
	{
		private readonly Rgba32? c_value;
        private readonly string? s_value; // Rgba32 | string

		public CssColor(string value)
			: base(NodeKind.Color)
		{
			this.s_value = value;
		}

		public CssColor(Rgba32 value)
			: base(NodeKind.Color)
		{
            this.c_value = value;
		}

        internal override void WriteTo(TextWriter writer)
        {
			writer.Write(ToString());
        }

        public override string ToString()
		{
			return c_value.HasValue
				? c_value.ToString()!
				: s_value!;
		}

		public static CssColor FromRgba(byte r, byte g, byte b, float a)
		{
			return new CssColor($"rgba({r}, {g}, {b}, {a})");
		}

		public override CssNode CloneNode()
		{
			return c_value.HasValue
				? new CssColor(c_value.Value)
				: new CssColor(s_value!);
		}
    }
}

// hsl
// rgba
// hsla
// #hex
// named (purple, yellow, orange)