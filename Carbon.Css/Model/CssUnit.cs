namespace Carbon.Css
{
	public class CssUnit
	{
		private static readonly CssUnit PX = new CssUnit("px", NodeKind.Length);

		private readonly string name;
		private readonly NodeKind kind;

		public CssUnit(string name, NodeKind kind)
		{
			this.name = name;
			this.kind = kind;
		}

		public string Name => name;

		public NodeKind Kind => kind;

		public static CssUnit Get(string name)
		{
			switch (name)
			{
				// <length>
				case "em":
				case "ex":
				case "cm":
				case "ch":
				case "rem":
				case "vh":
				case "vw":
				case "vmin":
				case "vmax":
				case "px": 
				case "mm":
				case "in":
				case "pt":
				case "pc": return new CssUnit(name, NodeKind.Length);

				// <percentage>
				case "%": return new CssUnit(name, NodeKind.Percentage);

				// <angle>
				case "deg":
				case "grad":
				case "rad":
				case "turn": return new CssUnit(name, NodeKind.Angle);

				// <time>
				case "s":
				case "ms": return new CssUnit(name, NodeKind.Time);
				
				// <frequency>
				case "Hz":
				case "kHz": return new CssUnit(name, NodeKind.Frequency);
				
				// <resolution>
				case "dpi":
				case "dpcm":
				case "dppx": return new CssUnit(name, NodeKind.Resolution);

				default: return new CssUnit(name, NodeKind.Unknown);
				
            }
		}
	}
}

// <length>
// em, ex, cm, ch, rem, vh, vw, vmin, vmax
// px, mm, cm, in, pt, pc

// Percentages
// %

// <angle>
// deg, grad, rad, turn

// <time>
// s, ms

// <frequency>
// Hz, kHz

// <resolution>
// dpi, dpcm, dppx
