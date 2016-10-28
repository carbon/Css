using System;

namespace Carbon.Color
{
    public struct Rgba : IEquatable<Rgba>
    {
        public static readonly Rgba White   = new Rgba(255, 255, 255, 1);
        public static readonly Rgba Black   = new Rgba(0, 0, 0, 1);
        public static readonly Rgba Red     = new Rgba(255, 0, 0, 1);
        public static readonly Rgba Green   = new Rgba(0, 255, 0, 1);
        public static readonly Rgba Blue    = new Rgba(0, 0, 255, 1);

        public Rgba(byte r, byte g, byte b, float a = 1)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public byte R { get; }
        public byte G { get; }
        public byte B { get; }
        public float A { get; }

        public Hsla ToHsla() => Hsla.FromRgb(this);

        public Hsv ToHsv() => Hsv.FromColor(this);

        public Rgba WithOpacity(float alpha) => new Rgba(R, G, B, alpha);

        public Rgba Lighten(float amount)
        {
            return ToHsla().AdjustLightness(amount).ToRgb();

            // return this.Lerp(WebColor.White, amount);
        }

        public Rgba Darken(float amount)
        {
            return ToHsla().AdjustLightness(-amount).ToRgb();

            // return this.Lerp(WebColor.Black, amount);
        }

        public Rgba Desaturate(float amount)
        {
            return ToHsla().AdjustSaturation(-amount).ToRgb();
        }

        public Rgba Saturate(float amount)
        {
            return ToHsla().AdjustSaturation(amount).ToRgb();
        }

        public Rgba Lerp(Rgba to, float amount)
        {
            float sr = this.R, sg = this.G, sb = this.B;

            float er = to.R, eg = to.G, eb = to.B;

            return new Rgba(
               r: (byte)sr.Lerp(er, amount),
               g: (byte)sg.Lerp(eg, amount),
               b: (byte)sb.Lerp(eb, amount)
            );
        }

        public Rgba BlendWith(Rgba other, float amount = 1)
        {
            byte r = (byte)((R * amount) + other.R * (1f - amount));
            byte g = (byte)((G * amount) + other.G * (1f - amount));
            byte b = (byte)((B * amount) + other.B * (1f - amount));

            return new Rgba(r, g, b);
        }

        public string ToHex()
        {
            return ToHex(R) + ToHex(G) + ToHex(B);
        }

        private string ToHex(byte value)
        {
            return string.Concat("0123456789abcdef"[(value - value % 16) / 16], "0123456789abcdef"[value % 16]);
        }

        public string ToRgbString() => $"rgb({R}, {G}, {B})";

        // rgba(0,0,255,0.3)
        public string ToRgbaString() => $"rgba({R}, {G}, {B}, {A})";

        public override string ToString()
        {
            if (A != 1f) return ToRgbaString();

            return '#' + ToHex();
        }

        public static Rgba Parse(string text)
        {
            #region Preconditions

            if (text == null) throw new ArgumentNullException(nameof(text));

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

                    return new Rgba(data[0], data[1], data[2]);
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

                return new Rgba(
                    r: byte.Parse(parts[0]),
                    g: byte.Parse(parts[1]),
                    b: byte.Parse(parts[2]),
                    a: float.Parse(parts[3])
                );
            }
            else
            {
                string hex;

                if (NamedColor.TryGetHex(text, out hex))
                {
                    return Parse('#' + hex);
                }

                throw new Exception("Unexpected color:" + text);
            }
        }

        #region Equality

        public override bool Equals(object value)
        {
            if (!(value is Rgba)) return false;

            var obj = (Rgba)value;

            return obj.R == R && obj.G == G && obj.B == B && obj.A == A; ;
        }

        public bool Equals(Rgba obj)
            => obj.R == R && obj.G == G && obj.B == B && obj.A == A;

        public static bool operator ==(Rgba a, Rgba b) => a.Equals(b);

        public static bool operator !=(Rgba a, Rgba b) => !a.Equals(b);

        public override int GetHashCode() => R ^ G ^ B;

        #endregion
    }
}