using System.Linq;


namespace Carbon.Color
{
    public struct Hsv
    {
        public Hsv(float h, float s, float v)
        {
            H = h;
            S = s;
            V = v;
        }

        public float H { get; }
        public float S { get; }
        public float V { get; }

        public static Hsv FromColor(Rgba color)
        {
            // Clamp to 0-1
            float r = color.R / 255f,
                  b = color.B / 255f,
                  g = color.G / 255f;

            var max = new[] { r, g, b }.Max();
            var min = new[] { r, g, b }.Min();
            var d = max - min;

            float h = 0f,
                  s = 0f,
                  v = max;

            if (max != 0) s = d / max;

            if (max == min)
            {
                h = 0f;
            }
            else
            {
                if (max == r) h = (g - b) / d + (g < b ? 6 : 0);
                else if (max == g) h = (b - r) / d + 2;
                else if (max == b) h = (r - g) / d + 4;

                h /= 6f;
            }

            return new Hsv(h, s, v);
        }
    }
}
