using System;
using Carbon.Color;

namespace Carbon.Css.Gradients
{
    public readonly struct LinearColorStop
    {
        public LinearColorStop(Rgba32 color, double? position)
        {
            Color = color;
            Position = position;
        }

        public Rgba32 Color { get; }

        public double? Position { get; }
        
        public static LinearColorStop Parse(ReadOnlySpan<char> text)
        {
            if (text.Length == 0)
            {
                throw new ArgumentException("May not be empty", nameof(text));
            }

            // #000 
            // rgba(255, 255, 255, 50%) 50%


            int read = 0;

            while (read < text.Length && text[read] != ' ')
            {
                read++;
            }

            var color = text.Slice(0, read);

            double? angle = null;

            if (read != text.Length)
            {
                text = text.Slice(read);

                if (text[0] == ' ')
                {
                    text = text.Slice(1);
                }

                if (text.Length > 0)
                {
                    angle = double.Parse(text.Slice(0, text.Length - 1).ToString()) / 100d;
                }
            }

            return new LinearColorStop(Rgba32.Parse(color.ToString()), angle);
        }

        public override string ToString()
        {
            return Color.ToHex6() + " " + Position.Value.ToString("0%");
        }
    }
}