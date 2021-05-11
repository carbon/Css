using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json.Serialization;

using Carbon.Css.Json;

namespace Carbon.Css
{
    [JsonConverter(typeof(ThinknessJsonConverter))]
    public sealed class Thickness // of the margin, border, or padding
    {
        public Thickness(CssUnitValue value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Top = value;
            Left = value;
            Bottom = value;
            Right = value;
        }

        public Thickness(CssUnitValue top, CssUnitValue left, CssUnitValue bottom, CssUnitValue right)
        {
            Top = top ?? throw new ArgumentNullException(nameof(top));
            Left = left ?? throw new ArgumentNullException(nameof(top));
            Bottom = bottom ?? throw new ArgumentNullException(nameof(top));
            Right = right ?? throw new ArgumentNullException(nameof(top));
        }

        public CssUnitValue Top { get; }

        public CssUnitValue Left { get; }

        public CssUnitValue Bottom { get; }

        public CssUnitValue Right { get; }

        public static bool TryParse(
            [NotNullWhen(true)] string? value, 
            [NotNullWhen(true)] out Thickness? result)
        {
            if (value is null || value.Length == 0)
            {
                result = default;
                return false;
            }

            try
            {
                result = Parse(value);

                return true;
            }
            catch
            {
                result = default;
                return false;
            }
        }

        public static Thickness Parse(string value)
        {
            var top    = CssUnitValue.Zero;
            var left   = CssUnitValue.Zero;
            var bottom = CssUnitValue.Zero;
            var right  = CssUnitValue.Zero;

            value = value.Trim();

            string[] parts = value.Split(' ');

            for (int i = 0; i < parts.Length; i++)
            {
                var part = CssUnitValue.Parse(parts[i]);

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
                        case 2: bottom = part; break;
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
            if (Top.Equals(Bottom) && Left.Equals(Right))
            {
                if (Top.Equals(Left))
                {
                    return Top.ToString();
                }
                else
                {
                    return Top.ToString() + " " + Left.ToString();
                }
            }
      
            var sb = StringBuilderCache.Aquire();

            Top.WriteTo(sb);

            sb.Append(' ');
            Left.WriteTo(sb);

            sb.Append(' ');
            Bottom.WriteTo(sb);
            
            if (!ReferenceEquals(Left, Right))
            {
                sb.Append(' ');
                Right.WriteTo(sb);
            }

            return StringBuilderCache.ExtractAndRelease(sb);
        }
    }
}
