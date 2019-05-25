using System;
using System.Text;

namespace Carbon.Css
{
    public class Thickness // of the margin, border, or padding
    {
        public Thickness(CssUnitValue top, CssUnitValue left, CssUnitValue bottom, CssUnitValue right)
        {
            Top = top;
            Left = left;
            Bottom = bottom;
            Right = right;
        }

        public CssUnitValue Top { get; }

        public CssUnitValue Left { get; }

        public CssUnitValue Bottom { get; }

        public CssUnitValue Right { get; }

        public static Thickness Parse(string value)
        {
            var top = CssUnitValue.Zero;
            var left = CssUnitValue.Zero;
            var bottom = CssUnitValue.Zero;
            var right = CssUnitValue.Zero;

            value = value.Trim();

            string[] parts = value.Split(Seperators.Space);

            for (int i = 0; i < parts.Length; i++)
            {
                var part = CssUnitValue.Parse(parts[i].AsSpan());

                if (parts.Length == 1)
                {
                    top = part;
                    left = part;
                    bottom = part;
                    right = part;
                }
                else if (parts.Length == 2)
                {
                    // padding: 10px 20px;
                    // 10px = top & bottom
                    // 20px = left & right

                    switch (i)
                    {
                        case 0: top = part; bottom = part; break;
                        case 1: left = part; right = part; break;
                    }
                }
                else if (parts.Length == 3)
                {
                    // padding: 10px 3% 20px;
                    // 10px = top
                    // 3%   = left & right
                    // 20px = bottom

                    switch (i)
                    {
                        case 0: top = part; break;
                        case 1: left = part; right = part; break;
                        case 3: bottom = part; break;
                    }
                }
                else
                {
                    switch (i)
                    {
                        case 0: top = part; break;
                        case 1: left = part; break;
                        case 2: bottom = part; break;
                        case 3: right = part; break;
                    }
                }
            }

            return new Thickness(top, left, bottom, right);
        }

        public override string ToString()
        {
            if (Left == Top && Bottom == Top && Right == Top)
            {
                return Top.ToString();
            }

            var sb = StringBuilderCache.Aquire()
                .Append(Top?.ToString() ?? "0").Append(' ')
                .Append(Left?.ToString() ?? "0").Append(' ')
                .Append(Bottom?.ToString() ?? "0").Append(' ')
                .Append(Right?.ToString() ?? "0");

            return StringBuilderCache.ExtractAndRelease(sb);
        }
    }
}
