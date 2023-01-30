using System.Text.Json.Serialization;

namespace Carbon.Css;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CssUnitType
{
    None = 0, // Number | Percentage

    // Length
    Cm	    = 1, // centimeters	            1cm = 96px/2.54
    Mm	    = 2, // millimeters	            1mm = 1/10th of 1cm
    Q	    = 3, // quarter-millimeters	    1Q = 1/40th of 1cm
    In	    = 4, // inches	                1in = 2.54cm = 96px
    Pc	    = 5, // picas	                1pc = 1/6th of 1in
    Pt	    = 6, // points	                1pt = 1/72th of 1in
    Px	    = 7, // pixels	                1px = 1/96th of 1in

    // Length     // relative to
    Em      = 10, // font size of the element
    Ex      = 11, // x-height of the element’s font
    Cap     = 12, // cap height (the nominal height of capital letters) of the element’s font
    Ch      = 13, // average character advance of a narrow glyph in the element’s font, as represented by the “0” (ZERO, U+0030) glyph
    Ic      = 14, // average character advance of a fullwidth glyph in the element’s font, as represented by the “水” (CJK water ideograph, U+6C34) glyph
    Rem     = 15, // font size of the root element
    Lh      = 16, // line height of the element
    Rlh     = 17, // line height of the root element
    Vw	    = 18, // 1% of viewport’s width
    Vh	    = 19, // 1% of viewport’s height
    Vi	    = 20, // 1% of viewport’s size in the root element’s inline axis
    Vb	    = 21, // 1% of viewport’s size in the root element’s block axis
    Vmin    = 22, // 1% of viewport’s smaller dimension
    Vmax    = 23, // 1% of viewport’s larger dimension

    Cqw     = 24, // 1% of a query container's width
    Cqh     = 25, // 1% of a query container's height
    Cqi     = 26, // 1% of a query container's inline size
    Cqb     = 27, // 1% of a query container's block size
    Cqmin   = 28, // The smaller value of either cqi or cqb
    Cqmax   = 29, // The larger value of either cqi or cqb

    // Angle
    Deg     = 30,
    Grad    = 31,
    Rad     = 32,
    Turn    = 33,

    // Time
    S       = 34,
    Ms      = 35,

    // Frequency
    H       = 40,
    Khz     = 41,

    // Resolution
    Dpi     = 50,
    Dpcm    = 51,
    Dppx    = 52,
    X       = 53, // aka Dppx

    Custom = 100
}

// https://www.w3.org/TR/css-values/