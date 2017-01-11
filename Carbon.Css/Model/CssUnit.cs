namespace Carbon.Css
{
    public struct CssUnit
    {
        private static readonly CssUnit CH   = new CssUnit("ch",   NodeKind.Length);
        private static readonly CssUnit CM   = new CssUnit("cm",   NodeKind.Length);
        private static readonly CssUnit EM   = new CssUnit("em",   NodeKind.Length);
        private static readonly CssUnit EX   = new CssUnit("ex",   NodeKind.Length);
        private static readonly CssUnit PX   = new CssUnit("px",   NodeKind.Length);
        private static readonly CssUnit REM  = new CssUnit("rem",  NodeKind.Length);
        private static readonly CssUnit VH   = new CssUnit("vh",   NodeKind.Length);
        private static readonly CssUnit VW   = new CssUnit("vw",   NodeKind.Length);
        private static readonly CssUnit VMIN = new CssUnit("vmin", NodeKind.Length);

        private static readonly CssUnit Percentage = new CssUnit("%", NodeKind.Percentage);

        private static readonly CssUnit S = new CssUnit("s", NodeKind.Time);
        private static readonly CssUnit MS = new CssUnit("ms", NodeKind.Time);

        internal CssUnit(string name, NodeKind kind)
        {
            Name = name;
            Kind = kind;
        }

        public string Name { get; }

        public NodeKind Kind { get; }

        public static CssUnit Get(string name)
        {
            switch (name)
            {
                // <length>
                case "ch": return CH;    // width of the "0" (ZERO, U+0030) glyph in the element's font
                case "cm": return CM;
                case "em": return EM;    // font size of the element
                case "ex": return EX;    // x-height of the element's font
                case "rem": return REM;   // font size of the root element
                case "vh": return VH;
                case "vw": return VW;
                case "vmin": return VMIN;
                case "vmax":
                case "px":
                case "mm":
                case "in":
                case "pt":
                case "pc": return new CssUnit(name, NodeKind.Length);

                // <percentage>
                case "%": return Percentage;

                // <angle>
                case "deg":
                case "grad":
                case "rad":
                case "turn": return new CssUnit(name, NodeKind.Angle);

                // <time>
                case "s": return S;
                case "ms": return MS;

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
