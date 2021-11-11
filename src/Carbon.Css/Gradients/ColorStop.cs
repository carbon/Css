#pragma warning disable IDE0057 // Use range operator

using System;
using System.Globalization;
using System.Text.Json.Serialization;

using Carbon.Color;
using Carbon.Css.Helpers;

namespace Carbon.Css.Gradients;

public readonly struct ColorStop
{
    public ColorStop(Rgba32 color, double? position)
    {
        Color = color;
        Position = position;
    }

    public readonly Rgba32 Color { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public readonly double? Position { get; }

    public static ColorStop Parse(ReadOnlySpan<char> text)
    {
        return Read(text, out _);
    }

    public static ColorStop Read(ReadOnlySpan<char> text, out int read)
    {
        if (text.Length == 0)
        {
            throw new ArgumentException("May not be empty", nameof(text));
        }

        if (text.TryReadWhitespace(out read))
        {
            text = text[read..];
        }

        Rgba32 color = text.ReadColor(out int colorRead);
        text = text[colorRead..];

        read += colorRead;

        // #000 
        // rgba(255, 255, 255, 50%) 50%

        double? angle = null;

        if (text.Length > 0)
        {
            int commaIndex = text.IndexOf(',');

            if (commaIndex > -1)
            {
                text = text.Slice(0, commaIndex);

                read += commaIndex;
            }
            else
            {
                read += text.Length;
            }

            text = text.Trim();

            if (text.Length > 0)
            {
                angle = double.Parse(text[0..^1], provider: CultureInfo.InvariantCulture) / 100d;
            }
        }

        return new ColorStop(color, angle);
    }

    public readonly override string ToString()
    {
        if (Position != null)
        {
            return string.Create(CultureInfo.InvariantCulture, $"{Color.ToHexString()} {Position.Value:0%}");
        }
        else
        {
            return Color.ToHexString();
        }
    }
}
