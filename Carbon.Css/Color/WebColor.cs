namespace Carbon.Css.Color
{
	using System;

	using System.Text;
	
	// TODO: Move all of this into CssColor
	public struct WebColor : IColor
	{
		private readonly byte r;
		private readonly byte g;
		private readonly byte b;

		private readonly float alpha; // Increase precision of alpha

		private static readonly WebColor White = new WebColor(255, 255, 255, 1);
		private static readonly WebColor Black = new WebColor(0, 0, 0, 1);
		private static readonly WebColor Red	= new WebColor(255, 0, 0, 1);
		private static readonly WebColor Green	= new WebColor(0, 255, 0, 1);
		private static readonly WebColor Blue	= new WebColor(0, 0, 255, 1);

		public WebColor(Bgra c)
			: this(r: c.R,
				   g: c.G,
				   b: c.B,
				   a: (float)c.A / (float)255) { }

		public WebColor(byte r, byte g, byte b, float a = 1)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.alpha = a;
		}

		public byte R { get { return r; } }
		public byte G { get { return g; } }
		public byte B { get { return b; } }

		public float Alpha { get { return alpha; } }

		public Hsla ToHsla()
		{
			return Hsla.FromRgb(this);
		}

		public Hsv ToHsv()
		{
			return Hsv.FromColor(this);
		}

		public WebColor WithOpacity(float alpha)
		{
			return new WebColor(r, g, b, alpha);
		}

		public WebColor Lighten(float amount)
		{
			return this.Lerp(WebColor.White, amount);
		}

		public WebColor Lighten2(float amount)
		{
			var hsl = Hsla.FromRgb(this);

			var delta = 1 - hsl.L;

			// var l = hsl.L.Lerp(1d, amount);

			var l = hsl.L + (Math.Min(amount, 1f) * delta);

			return hsl.WithL(l).ToRgb();
		}

		public WebColor Darken(float amount)
		{
			return this.Lerp(WebColor.Black, amount);
		}

		public WebColor Darken2(float amount)
		{
			var hsl = Hsla.FromRgb(this);

			var l = hsl.L - (Math.Min(amount, 1f) * hsl.L);

			return hsl.WithL(l).ToRgb();
		}

		public WebColor Desaturate(float amount)
		{
			var hsl = Hsla.FromRgb(this);

			return hsl.Desaturate(amount).ToRgb();
		}

		public WebColor Saturate(float amount)
		{
			var hsl = Hsla.FromRgb(this);

			return hsl.Saturate(amount).ToRgb();
		}

		public WebColor Lerp(WebColor to, float amount)
		{
			float sr = this.R, sg = this.G, sb = this.B;

			float er = to.R, eg = to.G, eb = to.B;

			return new WebColor(
			   r: (byte)sr.Lerp(er, amount),
			   g: (byte)sg.Lerp(eg, amount),
			   b: (byte)sb.Lerp(eb, amount)
			);
		}

		

		public string ToHex()
		{
			return ToHex(r) + ToHex(g) + ToHex(b);
		}

		public string ToHex(byte value)
		{
			return string.Concat("0123456789abcdef"[(value - value % 16) / 16], "0123456789abcdef"[value % 16]);
		}

		public string ToRgb()
		{
			// background-color:rgba(0,0,255,0.3);

			return string.Format("rgb({0}, {1}, {2}, {3})", r, g, b);
		}

		public string ToRgba()
		{
			// background-color:rgba(0,0,255,0.3);

			return string.Format("rgba({0}, {1}, {2}, {3})", r, g, b, alpha);
		}

		public override string ToString()
		{
			if (alpha != 1f)
			{
				return ToRgba();
			}

			return '#' + ToHex();
		}


		public static WebColor Parse(string text)
		{
			#region Preconditions

			if (text == null) throw new ArgumentNullException("hex");

			#endregion

			if (text.StartsWith("#"))
			{
				var hex = text.TrimStart('#');

				// Support 3 letter hexes
				if (hex.Length == 3)
				{
					var newHex = new string(new[] { hex[0], hex[0], hex[1], hex[1], hex[2], hex[2] });

					hex = newHex;
				}

				try
				{
					var data = HexString.ToBytes(hex);

					return new WebColor(data[0], data[1], data[2]);
				}
				catch
				{
					throw new Exception("invalid color:" + text);
				}
			}
			else if (text.StartsWith("rgba("))
			{
				// rgba(197, 20, 37, 0.3)

				var parts = text.Substring(5).TrimEnd(')').Split(',');

				if (parts.Length != 4) throw new Exception("Must be 4 parts");

				return new WebColor(
					r: Byte.Parse(parts[0]),
					g: Byte.Parse(parts[1]),
					b: Byte.Parse(parts[2]),
					a: float.Parse(parts[3])
				);
			}
			else
			{
				throw new Exception("Unexpected color:" + text);
			}

			
		}
	}

	internal static class HexString
	{
		internal static string FromBytes(byte[] bytes)
		{
			#region Preconditions

			if (bytes == null) throw new ArgumentNullException("bytes");

			#endregion

			var sb = new StringBuilder(bytes.Length * 2);

			foreach (byte b in bytes)
			{
				sb.Append(b.ToString("x2"));
			}

			return sb.ToString();
		}

		internal static byte[] ToBytes(string hexString)
		{
			#region Preconditions

			if (hexString == null) throw new ArgumentNullException("hexString");

			if (hexString.Length % 2 != 0) throw new ArgumentException("Must be divisible by 2");

			#endregion

			byte[] bytes = new byte[hexString.Length / 2];

			for (int i = 0; i < hexString.Length; i += 2)
			{
				bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
			}

			return bytes;
		}
	}
}
