using System;

namespace Carbon.Color
{
    public struct Hsla
    {
        public Hsla(float h, float s, float l, float a = 1f)
        {
            H = h;
            S = s;
            L = l;
            A = a;
        }

        public float H { get;}
        public float S { get;}
        public float L { get;}
        public float A { get; }

        public override string ToString()
        {
            return string.Format("hsla({0},{1}%,{2}%,{3})", (
                /*0*/ (int)Math.Round(H * 360f)),
                /*1*/ (int)Math.Round(S * 100f),
                /*2*/ (int)Math.Round(L * 100f),
                /*3*/ 1
            );
        }

        public Hsla AdjustLightness(float value) => new Hsla(H, S, Constrain(L + value), A);

        public Hsla RotateHue(float degrees) => WithHueDegrees(HueDegrees + degrees);

        public Hsla AdjustSaturation(float value)
        {
            // var newValue = s.Lerp(1f, value);

            return WithS(Constrain(S + value));
        }

        public Hsla WithL(float l) 
            => new Hsla(H, S, l, A);

        public Hsla WithS(float s) 
            => new Hsla(H, s, L, A);

        public Hsla WithHue(float h) 
            => new Hsla(h, S, L, A);

        public float HueDegrees
        {
            get
            {
                // The H value returned should be from 0 to 6 so to convert it to degrees you just multiply by 60.
                // H can actually be negative sometimes so if it is just add 360;

                var degrees = H * 60f;

                if (degrees < 0) degrees += 360;

                return degrees;
            }
        }

        public Hsla WithHueDegrees(float degrees)
        {
            degrees = (degrees % 360f);

            // - 360f
            var value = degrees / 60f;

            // 0 - 6
            return WithHue(value);
        }

        private float Constrain(float value)
        {
            if (value > 1) return 1f;
            if (value < 0) return 0f;

            return value;
        }

        public Rgba ToRgb()
        {
            byte r, g, b;

            if (S == 0d)
            {
                r = (byte)Math.Round(L * 255f);
                g = (byte)Math.Round(L * 255f);
                b = (byte)Math.Round(L * 255f);
            }
            else
            {
                float t1, t2;
                float th = H / 6.0f;

                if (L < 0.5f)
                {
                    t2 = L * (1f + S);
                }
                else
                {
                    t2 = (L + S) - (L * S);
                }

                t1 = 2f * L - t2;

                float tr, tg, tb;

                tr = th + (1.0f / 3.0f);
                tg = th;
                tb = th - (1.0f / 3.0f);

                tr = ColorCalc(tr, t1, t2);
                tg = ColorCalc(tg, t1, t2);
                tb = ColorCalc(tb, t1, t2);

                r = (byte)Math.Round(tr * 255f);
                g = (byte)Math.Round(tg * 255f);
                b = (byte)Math.Round(tb * 255f);
            }
            return new Rgba(r, g, b, A);
        }

        public static Hsla FromRgb(Rgba color)
        {
            float r = (color.R / 255f),
                  g = (color.G / 255f),
                  b = (color.B / 255f);

            float min = Math.Min(Math.Min(r, g), b);
            float max = Math.Max(Math.Max(r, g), b);
            float delta = max - min;

            float h = 0,
                  s = 0,
                  l = (max + min) / 2.0f;

            if (delta != 0)
            {
                if (l < 0.5f)
                {
                    s = delta / (max + min);
                }
                else
                {
                    s = delta / (2.0f - max - min);
                }

                if (r == max)
                {
                    h = (g - b) / delta;
                }
                else if (g == max)
                {
                    h = 2f + (b - r) / delta;
                }
                else if (b == max)
                {
                    h = 4f + (r - g) / delta;
                }
            }

            return new Hsla(h, s, l, color.A);
        }

        private static float ColorCalc(float c, float t1, float t2)
        {
            if (c < 0) c += 1f;
            if (c > 1) c -= 1f;
            if (6.0f * c < 1.0f) return t1 + (t2 - t1) * 6.0f * c;
            if (2.0f * c < 1.0f) return t2;
            if (3.0f * c < 2.0f) return t1 + (t2 - t1) * (2.0f / 3.0f - c) * 6.0f;

            return t1;
        }
    }
}