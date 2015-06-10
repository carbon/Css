namespace Carbon.Css
{
	public class CssUnit
	{
		private static readonly CssUnit CH	= new CssUnit("ch", NodeKind.Length);
		private static readonly CssUnit CM	= new CssUnit("cm", NodeKind.Length);
		private static readonly CssUnit EM	= new CssUnit("em", NodeKind.Length);
        private static readonly CssUnit EX	= new CssUnit("ex", NodeKind.Length);
		private static readonly CssUnit PX	= new CssUnit("px", NodeKind.Length);
		private static readonly CssUnit REM = new CssUnit("rem", NodeKind.Length);
		private static readonly CssUnit VH	= new CssUnit("vh", NodeKind.Length);
		private static readonly CssUnit VW	= new CssUnit("vw", NodeKind.Length);

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
				case "ch"	: return CH;
				case "cm"	: return CM;
				case "em"	: return EM;
				case "ex"	: return EX;
				case "rem"	: return REM;
				case "vh"	: return VH;
				case "vw"	: return VW;
				case "vmin"	:
				case "vmax"	:
				case "px"	: 
				case "mm"	:
				case "in"	:
				case "pt"	:
				case "pc"	: return new CssUnit(name, NodeKind.Length);

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
// em, ex, cm, ch, rem, vh, vw, vmin, vmax					// ems, exs
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
