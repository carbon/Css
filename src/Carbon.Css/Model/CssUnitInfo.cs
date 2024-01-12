using System.Collections.Frozen;
using System.Runtime.CompilerServices;
using System.Text;

namespace Carbon.Css;

public sealed class CssUnitInfo(string name, NodeKind kind, CssUnitFlags flags = default) : IEquatable<CssUnitInfo>
{
    public static readonly CssUnitInfo Number = new (string.Empty, NodeKind.Number);

    // <length> (Relative) | em, ex, cm, ch, rem, vh, vw, vmin, vmax	                                         | relative to
    public static readonly CssUnitInfo Em    = new(CssUnitNames.Em,   NodeKind.Length, CssUnitFlags.Relative); // | font size of the element
    public static readonly CssUnitInfo Ex    = new(CssUnitNames.Ex,   NodeKind.Length, CssUnitFlags.Relative); // | x-height of the element’s font
    public static readonly CssUnitInfo Cap   = new("cap",             NodeKind.Length, CssUnitFlags.Relative); // | cap height (the nominal height of capital letters) of the element’s font
    public static readonly CssUnitInfo Ch    = new("ch",              NodeKind.Length, CssUnitFlags.Relative); // | average character advance of a narrow glyph in the element’s font, as represented by the “0” (ZERO, U+0030) glyph
    public static readonly CssUnitInfo Ic    = new("ic",              NodeKind.Length, CssUnitFlags.Relative); // | average character advance of a fullwidth glyph in the element’s font, as represented by the “水” (CJK water ideograph, U+6C34) glyph
    public static readonly CssUnitInfo Rem   = new(CssUnitNames.Rem,  NodeKind.Length, CssUnitFlags.Relative); // | font size of the root element
    public static readonly CssUnitInfo Lh    = new(CssUnitNames.Lh,   NodeKind.Length, CssUnitFlags.Relative); // | line height of the element
    public static readonly CssUnitInfo Rlh   = new(CssUnitNames.Rlh,  NodeKind.Length, CssUnitFlags.Relative); // | line height of the root element
    public static readonly CssUnitInfo Vw    = new(CssUnitNames.Vw,   NodeKind.Length, CssUnitFlags.Relative); // | 1% of viewport’s width
    public static readonly CssUnitInfo Vh    = new(CssUnitNames.Vh,   NodeKind.Length, CssUnitFlags.Relative); // | 1% of viewport’s height
    public static readonly CssUnitInfo Vi    = new(CssUnitNames.Vi,   NodeKind.Length, CssUnitFlags.Relative); // | 1% of viewport’s size in the root element’s inline axis
    public static readonly CssUnitInfo Vb    = new(CssUnitNames.Vb,   NodeKind.Length, CssUnitFlags.Relative); // | 1% of viewport’s size in the root element’s block axis
    public static readonly CssUnitInfo Vmin  = new(CssUnitNames.Vmin, NodeKind.Length, CssUnitFlags.Relative); // | 1% of viewport’s smaller dimension
    public static readonly CssUnitInfo Vmax  = new(CssUnitNames.Vmax, NodeKind.Length, CssUnitFlags.Relative); // | 1% of viewport’s larger dimension

    public static readonly CssUnitInfo Cqw   = new(CssUnitNames.Cqw,   NodeKind.Length, CssUnitFlags.Relative); // 1% of a query container's width
    public static readonly CssUnitInfo Cqh   = new(CssUnitNames.Cqh,   NodeKind.Length, CssUnitFlags.Relative); // 1% of a query container's height
    public static readonly CssUnitInfo Cqi   = new(CssUnitNames.Cqi,   NodeKind.Length, CssUnitFlags.Relative); // 1% of a query container's inline size
    public static readonly CssUnitInfo Cqb   = new(CssUnitNames.Cqb,   NodeKind.Length, CssUnitFlags.Relative); // 1% of a query container's block size
    public static readonly CssUnitInfo Cqmin = new(CssUnitNames.Cqmin, NodeKind.Length, CssUnitFlags.Relative); // The smaller value of either cqi or cqb
    public static readonly CssUnitInfo Cqmax = new(CssUnitNames.Cqmax, NodeKind.Length, CssUnitFlags.Relative); // The larger value of either cqi or cqb

    public static readonly CssUnitInfo Dvh   = new(CssUnitNames.Dvh,  NodeKind.Length, CssUnitFlags.Relative); // | 1% of viewport’s dynamic height
    public static readonly CssUnitInfo Dvw   = new(CssUnitNames.Dvw,  NodeKind.Length, CssUnitFlags.Relative); // | 1% of viewport’s dynamic width

    public static readonly CssUnitInfo Rcap  = new(CssUnitNames.Rcap, NodeKind.Length, CssUnitFlags.Relative); // | 1cap = height of a root element capital letter
    public static readonly CssUnitInfo Rex   = new(CssUnitNames.Rex,  NodeKind.Length, CssUnitFlags.Relative); // equal to the value of the ex unit on the root element.
    public static readonly CssUnitInfo Rch   = new(CssUnitNames.Rch,  NodeKind.Length, CssUnitFlags.Relative); // equal to the value of the ch unit on the root element
    public static readonly CssUnitInfo Ric   = new(CssUnitNames.Ric,  NodeKind.Length, CssUnitFlags.Relative); // equal to the value of the ic unit on the root element.


    // <length> | px, mm, cm, in, pt, pc
    public static readonly CssUnitInfo Cm    = new("cm",            NodeKind.Length); // centimeters	        1cm = 96px/2.54
    public static readonly CssUnitInfo Mm    = new("mm",            NodeKind.Length); // millimeters	        1mm = 1/10th of 1cm
    public static readonly CssUnitInfo Q     = new("q",             NodeKind.Length); // quarter-millimeters	1Q = 1/40th of 1cm
    public static readonly CssUnitInfo In    = new("in",            NodeKind.Length); // inches	                1in = 2.54cm = 96px
    public static readonly CssUnitInfo Pc    = new("pc",            NodeKind.Length); // picas	                1pc = 1/6th of 1in
    public static readonly CssUnitInfo Pt    = new("pt",            NodeKind.Length); // points	                1pt = 1/72th of 1in
    public static readonly CssUnitInfo Px    = new(CssUnitNames.Px, NodeKind.Length); // pixels	                1px = 1/96th of 1in

    // <percentages> | %
    public static readonly CssUnitInfo Percentage = new ("%", NodeKind.Percentage);

    // <angle> deg, grad, rad, turn
    public static readonly CssUnitInfo Deg  = new(CssUnitNames.Deg,  NodeKind.Angle);
    public static readonly CssUnitInfo Grad = new(CssUnitNames.Grad, NodeKind.Angle);
    public static readonly CssUnitInfo Rad  = new(CssUnitNames.Rad,  NodeKind.Angle);
    public static readonly CssUnitInfo Turn = new(CssUnitNames.Turn, NodeKind.Angle);

    // <time> | s, ms
    public static readonly CssUnitInfo S    = new(CssUnitNames.S,  NodeKind.Time);
    public static readonly CssUnitInfo Ms   = new(CssUnitNames.Ms, NodeKind.Time);

    // <frequency> | Hz, kHz
    public static readonly CssUnitInfo Hz   = new(CssUnitNames.Hz, NodeKind.Frequency);
    public static readonly CssUnitInfo Khz  = new("khz",           NodeKind.Frequency);

    // <resolution> | dpi, dpcm, dppx, x
    public static readonly CssUnitInfo Dpi  = new(CssUnitNames.Dpi, NodeKind.Resolution);
    public static readonly CssUnitInfo Dpcm = new("dpcm",           NodeKind.Resolution);
    public static readonly CssUnitInfo Dppx = new("dppx",           NodeKind.Resolution);
    public static readonly CssUnitInfo X    = new(CssUnitNames.X,   NodeKind.Resolution);

    public static readonly FrozenDictionary<string, CssUnitInfo> s_items = ((KeyValuePair<string, CssUnitInfo>[])[
        // <length> : relative
        new(CssUnitNames.Cap   , Cap),
        new(CssUnitNames.Em    , Em),
        new(CssUnitNames.Ex    , Ex),
        new("ch"               , Ch),
        new("ic"               , Ic),
        new(CssUnitNames.Rem   , Rem),
        new(CssUnitNames.Lh    , Lh),
        new(CssUnitNames.Rlh   , Rlh),
        new(CssUnitNames.Cqw   , Cqw),
        new(CssUnitNames.Cqh   , Cqh),
        new(CssUnitNames.Cqi   , Cqi),
        new(CssUnitNames.Cqb   , Cqb),
        new(CssUnitNames.Cqmin , Cqmin),
        new(CssUnitNames.Cqmax , Cqmax),
        new(CssUnitNames.Dvw   , Dvw),
        new(CssUnitNames.Dvh   , Dvh),
        new(CssUnitNames.Rcap  , Rcap),
        new(CssUnitNames.Vw    , Vw),
        new(CssUnitNames.Vh    , Vh),
        new(CssUnitNames.Vi    , Vi),
        new(CssUnitNames.Vb    , Vb),
        new(CssUnitNames.Vmin  , Vmin),
        new(CssUnitNames.Vmax  , Vmax),

        // <length> 
        new("cm"              , Cm),
        new("mm"              , Mm),
        new("q"               , Q),
        new("in"              , In),
        new("pc"              , Pc),
        new("pt"              , Pt),
        new(CssUnitNames.Px   , Px),
            
        // <percentage>
        new(CssUnitNames.Percent, Percentage),

        // <angle>
        new(CssUnitNames.Deg  , Deg),
        new(CssUnitNames.Grad , Grad),
        new(CssUnitNames.Rad  , Rad),
        new(CssUnitNames.Turn , Turn),

        // <time>
        new(CssUnitNames.S    , S),
        new(CssUnitNames.Ms   , Ms),

        // <frequency>
        new(CssUnitNames.Hz   , Hz),
        new("kHz"             , Khz),

        // <resolution>
        new(CssUnitNames.Dpi  , Dpi),
        new("dpcm"            , Dpcm),
        new("dppx"            , Dppx),
        new(CssUnitNames.X    , X)
    ]).ToFrozenDictionary();

    public string Name { get; } = name;

    public NodeKind Kind { get; } = kind;

    public CssUnitFlags Flags { get; } = flags;

    [SkipLocalsInit]
    public static CssUnitInfo Get(ReadOnlySpan<byte> utf8Bytes)
    {
        if (utf8Bytes.Length > CssUnitNames.MaxLength)
        {
            return new CssUnitInfo(Encoding.UTF8.GetString(utf8Bytes), NodeKind.Unknown);
        }

        Span<char> buffer = stackalloc char[CssUnitNames.MaxLength];

        Ascii.ToUtf16(utf8Bytes, buffer, out int length);

        return Get(buffer[0..length]);
    }

    public static CssUnitInfo Get(ReadOnlySpan<char> name)
    {
        if (name.Length is 1)
        {
            switch (name[0])
            {
                case '%': return Percentage;
                case 's': return S;
                case 'x': return X;
            }
        }
        else if (name.Length is 2)
        {
            switch (name)
            {
                case "px": return Px;
                case "pt": return Pt;
                case "em": return Em;
                case "lh": return Lh;
                case "ms": return Ms;
                case "vh": return Vh;
                case "vw": return Vw;
            }
        }
        else if (name.Length is 3)
        {
            switch (name)
            {
                case "cqb": return Cqb;
                case "cqh": return Cqh;
                case "cqi": return Cqi;
                case "cqw": return Cqw;
                case "deg": return Deg;
                case "dvh": return Dvh;
                case "dvw": return Dvw;
                case "rem": return Rem;
                case "rlh": return Rlh;
                case "rch": return Rch;
                case "rex": return Rex;
            }
        }
        else if (name.Length is 4)
        {
            switch (name)
            {
                case "rcap": return Rcap;
                case "vmax": return Vmax;
                case "vmin": return Vmin;
            }
        }
        else if (name.Length is 5)
        {
            switch (name)
            {
                case "cqmax": return Cqmin;
                case "cqmin": return Cqmax;
            }
        }

        string text = name.ToString();

        if (s_items.TryGetValue(text, out var unit))
        {
            return unit;
        }

        return new CssUnitInfo(text, NodeKind.Unknown);
    }

    public bool Equals(CssUnitInfo? other)
    {
        if (other is null) return this is null;

        return ReferenceEquals(this, other) || Name.Equals(other.Name, StringComparison.Ordinal);
    }

    public override bool Equals(object? obj)
    {
        return obj is CssUnitInfo other && Equals(other);
    }

    public override int GetHashCode() => Name.GetHashCode();
}   

// PERF notes: A dictionary lookup is ~1.67x faster than a large switch statement