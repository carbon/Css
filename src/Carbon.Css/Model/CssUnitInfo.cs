using System;
using System.Collections.Generic;

namespace Carbon.Css
{
    public sealed class CssUnitInfo : IEquatable<CssUnitInfo>
    {
        public static readonly CssUnitInfo Number = new CssUnitInfo(string.Empty, NodeKind.Number);

        // <length> (Relative) | em, ex, cm, ch, rem, vh, vw, vmin, vmax	                                  | relative to
        public static readonly CssUnitInfo Em   = new CssUnitInfo(CssUnitNames.Em,   NodeKind.Length, CssUnitFlags.Relative); // | font size of the element
        public static readonly CssUnitInfo Ex   = new CssUnitInfo("ex",              NodeKind.Length, CssUnitFlags.Relative); // | x-height of the element’s font
        public static readonly CssUnitInfo Cap  = new CssUnitInfo("cap",             NodeKind.Length, CssUnitFlags.Relative); // | cap height (the nominal height of capital letters) of the element’s font
        public static readonly CssUnitInfo Ch   = new CssUnitInfo("ch",              NodeKind.Length, CssUnitFlags.Relative); // | average character advance of a narrow glyph in the element’s font, as represented by the “0” (ZERO, U+0030) glyph
        public static readonly CssUnitInfo Ic   = new CssUnitInfo("ic",              NodeKind.Length, CssUnitFlags.Relative); // | average character advance of a fullwidth glyph in the element’s font, as represented by the “水” (CJK water ideograph, U+6C34) glyph
        public static readonly CssUnitInfo Rem  = new CssUnitInfo(CssUnitNames.Rem,  NodeKind.Length, CssUnitFlags.Relative); // | font size of the root element
        public static readonly CssUnitInfo Lh   = new CssUnitInfo("lh",              NodeKind.Length, CssUnitFlags.Relative); // | line height of the element
        public static readonly CssUnitInfo Rlh  = new CssUnitInfo("rlh",             NodeKind.Length, CssUnitFlags.Relative); // | line height of the root element
        public static readonly CssUnitInfo Vw   = new CssUnitInfo("vw",              NodeKind.Length, CssUnitFlags.Relative); // | 1% of viewport’s width
        public static readonly CssUnitInfo Vh   = new CssUnitInfo("vh",              NodeKind.Length, CssUnitFlags.Relative); // | 1% of viewport’s height
        public static readonly CssUnitInfo Vi   = new CssUnitInfo("vi",              NodeKind.Length, CssUnitFlags.Relative); // | 1% of viewport’s size in the root element’s inline axis
        public static readonly CssUnitInfo Vb   = new CssUnitInfo("vb",              NodeKind.Length, CssUnitFlags.Relative); // | 1% of viewport’s size in the root element’s block axis
        public static readonly CssUnitInfo Vmin = new CssUnitInfo(CssUnitNames.Vmin, NodeKind.Length, CssUnitFlags.Relative); // | 1% of viewport’s smaller dimension
        public static readonly CssUnitInfo Vmax = new CssUnitInfo(CssUnitNames.Vmax, NodeKind.Length, CssUnitFlags.Relative); // | 1% of viewport’s larger dimension

        // <length> | px, mm, cm, in, pt, pc
        public static readonly CssUnitInfo Cm = new CssUnitInfo("cm",            NodeKind.Length); // centimeters	            1cm = 96px/2.54
        public static readonly CssUnitInfo Mm = new CssUnitInfo("mm",            NodeKind.Length); // millimeters	            1mm = 1/10th of 1cm
        public static readonly CssUnitInfo Q  = new CssUnitInfo("q",             NodeKind.Length); // quarter-millimeters	    1Q = 1/40th of 1cm
        public static readonly CssUnitInfo In = new CssUnitInfo("in",            NodeKind.Length); // inches	                1in = 2.54cm = 96px
        public static readonly CssUnitInfo Pc = new CssUnitInfo("pc",            NodeKind.Length); // picas	                1pc = 1/6th of 1in
        public static readonly CssUnitInfo Pt = new CssUnitInfo("pt",            NodeKind.Length); // points	                1pt = 1/72th of 1in
        public static readonly CssUnitInfo Px = new CssUnitInfo(CssUnitNames.Px, NodeKind.Length); // pixels	                1px = 1/96th of 1in

        // <percentages> | %
        public static readonly CssUnitInfo Percentage = new CssUnitInfo("%", NodeKind.Percentage);

        // <angle> deg, grad, rad, turn
        public static readonly CssUnitInfo Deg  = new CssUnitInfo(CssUnitNames.Deg,  NodeKind.Angle);
        public static readonly CssUnitInfo Grad = new CssUnitInfo("grad",            NodeKind.Angle);
        public static readonly CssUnitInfo Rad  = new CssUnitInfo("rad",             NodeKind.Angle);
        public static readonly CssUnitInfo Turn = new CssUnitInfo("turn",            NodeKind.Angle);

        // <time> | s, ms
        public static readonly CssUnitInfo S    = new CssUnitInfo(CssUnitNames.S,  NodeKind.Time);
        public static readonly CssUnitInfo Ms   = new CssUnitInfo(CssUnitNames.Ms, NodeKind.Time);

        // <frequency> | Hz, kHz
        public static readonly CssUnitInfo Hz   = new CssUnitInfo("Hz",  NodeKind.Frequency);
        public static readonly CssUnitInfo Khz  = new CssUnitInfo("khz", NodeKind.Frequency);

        // <resolution> | dpi, dpcm, dppx, x
        public static readonly CssUnitInfo Dpi   = new CssUnitInfo("dpi",          NodeKind.Resolution);
        public static readonly CssUnitInfo Dpcm  = new CssUnitInfo("dpcm",         NodeKind.Resolution);
        public static readonly CssUnitInfo Dppx  = new CssUnitInfo("dppx",         NodeKind.Resolution);
        public static readonly CssUnitInfo X     = new CssUnitInfo(CssUnitNames.X, NodeKind.Resolution);


        public static readonly Dictionary<string, CssUnitInfo> items = new Dictionary<string, CssUnitInfo> {
            // <length> : relative
            { CssUnitNames.Em   , Em },
            { "ex"              , Ex },
            { "cap"             , Cap },
            { "ch"              , Ch },
            { "ic"              , Ic },
            { CssUnitNames.Rem  , Rem },
            { "lh"              , Lh },
            { "rlh"             , Rlh },
            { "vw"              , Vw },
            { "vh"              , Vh },
            { "vi"              , Vi },
            { "vb"              , Vb },
            { CssUnitNames.Vmin , Vmin },
            { CssUnitNames.Vmax , Vmax },
                            
            // <length> 
            { "cm"              , Cm },
            { "mm"              , Mm },
            { "q"               , Q },
            { "in"              , In },
            { "pc"              , Pc },
            { "pt"              , Pt },
            { CssUnitNames.Px   , Px },
            
            // <percentage>
            { "%" , Percentage },

            // <angle>
            { CssUnitNames.Deg  , Deg },
            { "grad"            , Grad },
            { "rad"             , Rad },
            { "turn"            , Turn },

            // <time>
            { CssUnitNames.S    , S },
            { CssUnitNames.Ms   , Ms },

            // <frequency>
            { "Hz" , Hz },
            { "kHz", Khz },

            // <resolution>
            { "dpi"             , Dpi },
            { "dpcm"            , Dpcm },
            { "dppx"            , Dppx },
            { CssUnitNames.X    , X },
        };

        internal CssUnitInfo(string name, NodeKind kind, CssUnitFlags flags = CssUnitFlags.None)
        {
            Name = name;
            Kind = kind;
            Flags = flags;
        }

        public string Name { get; }

        public NodeKind Kind { get; }

        public CssUnitFlags Flags { get; }
        
        public static CssUnitInfo Get(string name)
        {
            if (ReferenceEquals(CssUnitNames.Px, name))
            {
                return Px;
            }
            else if (ReferenceEquals(CssUnitNames.Em, name))
            {
                return Em;
            }
            else if (ReferenceEquals(CssUnitNames.Percent, name))
            {
                return Percentage;
            }

            if (items.TryGetValue(name, out var unit))
            {
                return unit;
            }

            return new CssUnitInfo(name, NodeKind.Unknown);
        }

        public bool Equals(CssUnitInfo other)
        {
            return ReferenceEquals(this, other) || Name.Equals(other.Name, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }   
}

// PERF notes: A dictionary lookup is ~1.67x faster than a large switch statement