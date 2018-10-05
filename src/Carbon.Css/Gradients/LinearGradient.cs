using System;
using System.Collections.Generic;
using System.Text;
using Carbon.Css.Helpers;

namespace Carbon.Css.Gradients
{
    public readonly struct LinearGradient : IGradient
    {
        public LinearGradient(LinearGradientDirection direction, double? angle, LinearColorStop[] colorStops)
        {
            Direction = direction;
            Angle = angle;
            ColorStops = colorStops;
        }

        public LinearGradientDirection Direction { get; }

        public double? Angle { get; }

        // [ <linear-color-stop> [, <linear-color-hint>]? ]# , <linear-color-stop>
        public LinearColorStop[] ColorStops { get; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("linear-gradient(");

            if (Angle != null)
            {
                sb.Append(Angle.Value);
                sb.Append("deg");
            }
            else if (Direction != default)
            {
                sb.Append("to ");

                sb.Append(LinearGradientDirectionHelper.Canonicalize(Direction));
            }

            foreach (var stop in ColorStops)
            {
                sb.Append(", ");

                sb.Append(stop.Color.ToString());

                if (stop.Position is double position)
                {
                    sb.Append(' ');
                    sb.Append(position.ToString("0.##%"));
                }
            }

            sb.Append(')');

            return sb.ToString();
        }

        public static LinearGradient Parse(string text)
        {
            return Parse(text.AsSpan());
        }

        public static LinearGradient Parse(ReadOnlySpan<char> text)
        {
            if (text.StartsWith("linear-gradient(".AsSpan()))
            {
                text = text.Slice(16, text.Length - 17);
            }

            if (text.Length == 0)
            {
                throw new ArgumentException("May not be empty", nameof(text));
            }

            double? angle = null;
            LinearGradientDirection direction = default;

            if (char.IsDigit(text[0]))
            {
                angle = ReadAngle(text, out int read);

                text = text.Slice(read);
            }
            else if (LinearGradientDirectionHelper.TryParse(text, out direction, out int read))
            {
                text = text.Slice(read);
            }

            var colorStops = new List<LinearColorStop>();

            while (text.Length > 0)
            {
                if (TryReadWhitespace(text, out int read))
                {
                    text = text.Slice(read);
                }

                if (text[0] == ',')
                {
                    text = text.Slice(1);
                }

                int commaIndex = text.IndexOf(',');

                ReadOnlySpan<char> stopText;

                if (commaIndex > -1)
                {
                    stopText = text.Slice(0, commaIndex);

                    text = text.Slice(commaIndex + 1);
                }
                else
                {
                    // final stop...

                    stopText = text;

                    text = text.Slice(text.Length);
                }

                colorStops.Add(LinearColorStop.Parse(stopText.Trim()));
            }

            // TODO: Set the default stops 

            // // [ <angle> | to [top | bottom] || [left | right] ],]? <color-stop>[, <color-stop>]+);

            return new LinearGradient(direction, angle, colorStops.ToArray());
        }

        private static bool TryReadWhitespace(ReadOnlySpan<char> text, out int read)
        {
            read = 0;

            while (text.Length < read && text[read] == ' ')
            {
                read++;
            }

            return read > 0;
        }

        private static double ReadAngle(ReadOnlySpan<char> text, out int read)
        {
            var value = text.ReadNumber(out read);

            text = text.Slice(read);

            if (text.StartsWith("deg".AsSpan()))
            {
                read += 3;
            }

            return value;
        }
    }
}


// [ [ <angle> | to<side-or-corner>] ,]?

// https://medium.com/@patrickbrosset/do-you-really-understand-css-linear-gradients-631d9a895caf

// if the angle is omited, defaults to bottom (180deg)

// A linear gradient is defined by an axis—the gradient line—and two or more color-stop points. 

// 0deg points upwards
// 180deg points downward