using System.Runtime.Serialization;

namespace Carbon.Css;

// https://www.w3.org/TR/css-typed-om-1/#csspositionvalue
public readonly struct CssPosition(CssUnitValue x, CssUnitValue y)
{
    // e.g. 100px
    [DataMember(Name = "x")]
    public readonly CssUnitValue X { get; } = x;

    [DataMember(Name = "y")]
    public readonly CssUnitValue Y { get; } = y;
}