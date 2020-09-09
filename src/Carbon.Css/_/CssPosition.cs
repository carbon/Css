using System.Runtime.Serialization;

namespace Carbon.Css
{
    // https://www.w3.org/TR/css-typed-om-1/#csspositionvalue
    public readonly struct CssPosition
    {
        public CssPosition(CssUnitValue x, CssUnitValue y)
        {
            X = x;
            Y = y;
        }

        // e.g. 100px
        [DataMember(Name = "x")]
        public readonly CssUnitValue X { get; }

        [DataMember(Name = "y")]
        public readonly CssUnitValue Y { get; }
    }
}
