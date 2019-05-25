using System;
using System.Collections.Generic;
using System.Text;
using Carbon.Css.Helpers;

namespace Carbon.Css.Gradients
{
    public readonly struct LinearGradient : IGradient
    {
        public LinearGradient(LinearGradientDirection direction, double? angle, ColorStop[] colorStops)
        {
            Direction = direction;
            Angle = angle;
            Stops = colorStops;
        }

        public LinearGradientDirection Direction { get; }

        public double? Angle { get; }

        // [ <linear-color-stop> [, <linear-color-hint>]? ]# , <linear-color-stop>
        public ColorStop[] Stops { get; }

        public override string ToString()
        {
            var sb = StringBuilderCache.Aquire();

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

            foreach (var stop in Stops)
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

            return StringBuilderCache.ExtractAndRelease(sb);
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

            if (char.IsDigit(text[0]) || text[0] == '-')
            {
                angle = ReadAngle(text, out int read);

                text = text.Slice(read);
            }
            else if (LinearGradientDirectionHelper.TryParse(text, out direction, out int read))
            {
                text = text.Slice(read);
            }

            var colorStops = new List<ColorStop>();

            while (text.Length > 0)
            {
                if (text[0] == ',')
                {
                    text = text.Slice(1);
                }

                if (text.TryReadWhitespace(out int read))
                {
                    text = text.Slice(read);
                }

                if (text[0] == ',')
                {
                    text = text.Slice(1);
                }

                var colorStop = ColorStop.Read(text, out read);
                
                text = text.Slice(read);
                
                colorStops.Add(colorStop);
            }

            // TODO: Set the default stops 

            // // [ <angle> | to [top | bottom] || [left | right] ],]? <color-stop>[, <color-stop>]+);

            return new LinearGradient(direction, angle, colorStops.ToArray());
        }

        private static double ReadAngle(ReadOnlySpan<char> text, out int read)
        {
            double value = text.ReadNumber(out read);

            text = text.Slice(read);

            if (text.StartsWith("deg".AsSpan()))
            {
                read += 3;
            }

            return value;
        }
    }
}

/*
<linearGradient id="MyGradient">
    <stop offset="5%"  stop-color="green"/>
    <stop offset="95%" stop-color="gold"/>
</linearGradient>
*/

// [ [ <angle> | to<side-or-corner>] ,]?

// https://medium.com/@patrickbrosset/do-you-really-understand-css-linear-gradients-631d9a895caf

// if the angle is omited, defaults to bottom (180deg)

// A linear gradient is defined by an axis—the gradient line—and two or more color-stop points. 

// 0deg points upwards
// 180deg points downward
