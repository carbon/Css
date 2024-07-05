using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;

using Carbon.Css.Helpers;

namespace Carbon.Css.Gradients;

public readonly struct LinearGradient(
    LinearGradientDirection direction,
    double? angle,
    ColorStop[] colorStops) : IGradient
{
    [JsonPropertyName("direction")]
    public readonly LinearGradientDirection Direction { get; } = direction;

    [JsonPropertyName("angle")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public readonly double? Angle { get; } = angle;

    // [ <linear-color-stop> [, <linear-color-hint>]? ]# , <linear-color-stop>
    [JsonPropertyName("stops")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public readonly ColorStop[] Stops { get; } = colorStops;

    [SkipLocalsInit]
    public readonly override string ToString()
    {
        using var sb = new ValueStringBuilder(stackalloc char[64]);

        Span<char> buffer = new char[12];

        sb.Append("linear-gradient(");

        if (Angle is double angle)
        {
            sb.AppendSpanFormattable(angle, "0.######", CultureInfo.InvariantCulture);
            sb.Append("deg");
        }
        else if (Direction != default)
        {
            sb.Append("to ");

            sb.Append(LinearGradientDirectionHelper.Canonicalize(Direction));
        }

        for (int i = 0; i < Stops.Length; i++)
        {
            ref ColorStop stop = ref Stops[i];

            sb.Append(", ");

            if (stop.Color.IsOpaque)
            {
                stop.Color.TryFormatHexString(buffer, out int c);

                sb.Append(buffer[..c]);
            }
            else
            {
                sb.Append(stop.Color.ToString());
            }

            if (stop.Position is double position)
            {
                sb.Append(' ');
                sb.AppendSpanFormattable(position, "0.##%", CultureInfo.InvariantCulture);
            }
        }

        sb.Append(')');

        return sb.ToString();
    }

    public static LinearGradient Parse(string text)
    {
        return Parse(text.AsSpan());
    }

    public static bool TryParse(ReadOnlySpan<char> text, out LinearGradient result)
    {
        if (text.IsEmpty)
        {
            result = default;

            return false;
        }

        try
        {
            result = Parse(text);

            return true;
        }
        catch { }

        result = default;

        return false;
    }

    public static LinearGradient Parse(ReadOnlySpan<char> text)
    {
        if (text.StartsWith("linear-gradient(", StringComparison.Ordinal))
        {
            text = text[16..^1];
        }

        if (text.IsEmpty)
        {
            throw new ArgumentException("May not be empty", nameof(text));
        }

        double? angle = null;
        LinearGradientDirection direction = default;

        if (char.IsAsciiDigit(text[0]) || text[0] is '-')
        {
            angle = ReadAngle(text, out int read);

            text = text[read..];
        }
        else if (LinearGradientDirectionHelper.TryParse(text, out direction, out int read))
        {
            text = text[read..];
        }

        var colorStops = new List<ColorStop>();

        while (text.Length > 0)
        {
            if (text[0] is ',')
            {
                text = text[1..];
            }

            if (text.TryReadWhitespace(out int read))
            {
                text = text[read..];
            }

            if (text[0] is ',')
            {
                text = text[1..];
            }

            var colorStop = ColorStop.Read(text, out read);

            text = text[read..];

            colorStops.Add(colorStop);
        }

        // TODO: Set the default stops 

        // // [ <angle> | to [top | bottom] || [left | right] ],]? <color-stop>[, <color-stop>]+);

        return new LinearGradient(direction, angle, [.. colorStops]);
    }

    private static double ReadAngle(ReadOnlySpan<char> text, out int read)
    {
        double value = NumberHelper.ReadNumber(text, out read);

        if (text[read..].StartsWith("deg", StringComparison.Ordinal))
        {
            read += 3;
        }

        return value;
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

// if the angle is omitted, defaults to bottom (180deg)

// A linear gradient is defined by an axis—the gradient line—and two or more color-stop points. 

// 0deg points upwards
// 180deg points downward
