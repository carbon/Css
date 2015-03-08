namespace Carbon.Css.Color
{
	using System;
using System.Collections.Generic;
using System.Text;
	
	// TODO: Move all of this into CssColor
	public struct WebColor : IColor
	{
		private readonly byte r;
		private readonly byte g;
		private readonly byte b;

		private readonly float alpha; // Increase precision of alpha

		private static readonly WebColor White  = new WebColor(255, 255, 255, 1);
		private static readonly WebColor Black  = new WebColor(0, 0, 0, 1);
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
			return ToHsla().AdjustLightness(amount).ToRgb();
			
			// return this.Lerp(WebColor.White, amount);
		}


		public WebColor Darken(float amount)
		{
			return ToHsla().AdjustLightness(- amount).ToRgb();

			// return this.Lerp(WebColor.Black, amount);
		}

		public WebColor Desaturate(float amount)
		{
			return ToHsla().AdjustSaturation(- amount).ToRgb();
		}

		public WebColor Saturate(float amount)
		{

			return ToHsla().AdjustSaturation(amount).ToRgb();
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

		public WebColor BlendWith(WebColor other, float amount = 1)
		{
			byte r = (byte)((this.R * amount) + other.R * (1f - amount));
			byte g = (byte)((this.G * amount) + other.G * (1f - amount));
			byte b = (byte)((this.B * amount) + other.B * (1f - amount));

			return new WebColor(r, g, b);
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
				string hex;

				if (NameToHexMap.TryGetValue(text, out hex))
				{
					return WebColor.Parse('#' + hex);
				}

				throw new Exception("Unexpected color:" + text);
			}

			
		}


		private static readonly IDictionary<string, string> NameToHexMap = new Dictionary<string, string> {
			{ "aliceblue"            , "F0F8FF" },
			{ "antiquewhite"         , "FAEBD7" },
			{ "aquamarine"           , "7FFFD4" },
			{ "azure"                , "F0FFFF" },
			{ "beige"                , "F5F5DC" },
			{ "bisque"               , "FFE4C4" },
			{ "black"                , "000000" },
			{ "blanchedalmond"       , "FFEBCD" },
			{ "blue"                 , "0000FF" },
			{ "blueviolet"           , "8A2BE2" },
			{ "brown"                , "A52A2A" },
			{ "burlywood"            , "DEB887" },
			{ "cadetblue"            , "5F9EA0" },
			{ "chartreuse"           , "7FFF00" },
			{ "chocolate"            , "D2691E" },
			{ "coral"                , "FF7F50" },
			{ "cornflowerblue"       , "6495ED" },
			{ "cornsilk"             , "FFF8DC" },
			{ "crimson"              , "DC143C" },
			{ "cyan"                 , "00FFFF" },
			{ "darkblue"             , "00008B" },
			{ "darkcyan"             , "008B8B" },
			{ "darkgoldenrod"        , "B8860B" },
			{ "darkgray"             , "A9A9A9" },
			{ "darkgreen"            , "006400" },
			{ "darkkhaki"            , "BDB76B" },
			{ "darkmagenta"          , "8B008B" },
			{ "darkolivegreen"       , "556B2F" },
			{ "darkorange"           , "FF8C00" },
			{ "darkorchid"           , "9932CC" },
			{ "darkred"              , "8B0000" },
			{ "darksalmon"           , "E9967A" },
			{ "darkseagreen"         , "8FBC8F" },
			{ "darkslateblue"        , "483D8B" },
			{ "darkslategray"        , "2F4F4F" },
			{ "darkturquoise"        , "00CED1" },
			{ "darkviolet"           , "9400D3" },
			{ "deeppink"             , "FF1493" },
			{ "deepskyblue"          , "00BFFF" },
			{ "dimgray"              , "696969" },
			{ "dodgerblue"           , "1E90FF" },
			{ "firebrick"            , "B22222" },
			{ "floralwhite"          , "FFFAF0" },
			{ "forestgreen"          , "228B22" },
			{ "gainsboro"            , "DCDCDC" },
			{ "ghostwhite"           , "F8F8FF" },
			{ "gold"                 , "FFD700" },
			{ "goldenrod"            , "DAA520" },
			{ "gray"                 , "808080" },
			{ "green"                , "008000" },
			{ "greenyellow"          , "ADFF2F" },
			{ "honeydew"             , "F0FFF0" },
			{ "hotpink"              , "FF69B4" },
			{ "indianred"            , "CD5C5C" },
			{ "indigo"               , "4B0082" },
			{ "ivory"                , "FFFFF0" },
			{ "khaki"                , "F0E68C" },
			{ "lavender"             , "E6E6FA" },
			{ "lavenderblush"        , "FFF0F5" },
			{ "lawngreen"            , "7CFC00" },
			{ "lemonchiffon"         , "FFFACD" },
			{ "lightblue"            , "ADD8E6" },
			{ "lightcoral"           , "F08080" },
			{ "lightcyan"            , "E0FFFF" },
			{ "lightgoldenrodyellow" , "FAFAD2" },
			{ "lightgreen"           , "90EE90" },
			{ "lightgray"            , "D3D3D3" },
			{ "lightpink"            , "FFB6C1" },
			{ "lightsalmon"          , "FFA07A" },
			{ "lightseagreen"        , "20B2AA" },
			{ "lightskyblue"         , "87CEFA" },
			{ "lightslategray"       , "778899" },
			{ "lightsteelblue"       , "B0C4DE" },
			{ "lightyellow"          , "FFFFE0" },
			{ "lime"                 , "00FF00" },
			{ "limegreen"            , "32CD32" },
			{ "linen"                , "FAF0E6" },
			{ "magenta"              , "FF00FF" },
			{ "maroon"               , "800000" },
			{ "mediumaquamarine"     , "66CDAA" },
			{ "mediumblue"           , "0000CD" },
			{ "mediumorchid"         , "BA55D3" },
			{ "mediumpurple"         , "9370DB" },
			{ "mediumseagreen"       , "3CB371" },
			{ "mediumslateblue"      , "7B68EE" },
			{ "mediumspringgreen"    , "00FA9A" },
			{ "mediumturquoise"      , "48D1CC" },
			{ "mediumvioletred"      , "C71585" },
			{ "midnightblue"         , "191970" },
			{ "mintcream"            , "F5FFFA" },
			{ "mistyrose"            , "FFE4E1" },
			{ "moccasin"             , "FFE4B5" },
			{ "navajowhite"          , "FFDEAD" },
			{ "navy"                 , "000080" },
			{ "oldlace"              , "FDF5E6" },
			{ "olive"                , "808000" },
			{ "olivedrab"            , "6B8E23" },
			{ "orange"               , "FFA500" },
			{ "orangered"            , "FF4500" },
			{ "orchid"               , "DA70D6" },
			{ "palegoldenrod"        , "EEE8AA" },
			{ "palegreen"            , "98FB98" },
			{ "paleturquoise"        , "AFEEEE" },
			{ "palevioletred"        , "DB7093" },
			{ "papayawhip"           , "FFEFD5" },
			{ "peachpuff"            , "FFDAB9" },
			{ "peru"                 , "CD853F" },
			{ "pink"                 , "FFC0CB" },
			{ "plum"                 , "DDA0DD" },
			{ "powderblue"           , "B0E0E6" },
			{ "purple"               , "800080" },
			{ "red"                  , "FF0000" },
			{ "rebeccapurple"        , "663399" },
			{ "rosybrown"            , "BC8F8F" },
			{ "royalblue"            , "4169E1" },
			{ "saddlebrown"          , "8B4513" },
			{ "salmon"               , "FA8072" },
			{ "sandybrown"           , "F4A460" },
			{ "seagreen"             , "2E8B57" },
			{ "seashell"             , "FFF5EE" },
			{ "sienna"               , "A0522D" },
			{ "silver"               , "C0C0C0" },
			{ "skyblue"              , "87CEEB" },
			{ "slateblue"            , "6A5ACD" },
			{ "slategray"            , "708090" },
			{ "snow"                 , "FFFAFA" },
			{ "springgreen"          , "00FF7F" },
			{ "steelblue"            , "4682B4" },
			{ "tan"                  , "D2B48C" },
			{ "teal"                 , "008080" },
			{ "thistle"              , "D8BFD8" },
			{ "tomato"               , "FF6347" },
			{ "transparent"          , "000000" },
			{ "turquoise"            , "40E0D0" },
			{ "violet"               , "EE82EE" },
			{ "wheat"                , "F5DEB3" },
			{ "white"                , "FFFFFF" },
			{ "whitesmoke"           , "F5F5F5" },
			{ "yellow"               , "FFFF00" },
			{ "yellowgreen"          , "9ACD32" }
		};
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
