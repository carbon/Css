namespace Carbon.Css.Gradients;

using static LinearGradientDirection;

public static class LinearGradientDirectionHelper
{
    public static bool TryParse(ReadOnlySpan<char> text, out LinearGradientDirection result, out int read)
    {
        read = 0;

        if (text.Length < 3)
        {
            result = default;

            return false;
        }

        if (text.StartsWith("to ", StringComparison.Ordinal)) // 'to ' 
        {
            read += 3;

            text = text[3..];
        }

        if (text.StartsWith("top", StringComparison.Ordinal))
        {
            read += 3;

            text = text[3..];

            if (text.StartsWith(" left", StringComparison.Ordinal))
            {
                read += 5;

                result = TopLeft;
            }
            else if (text.StartsWith(" right", StringComparison.Ordinal))
            {
                read += 6;

                result = TopRight;
            }
            else
            {
                result = Top;
            }
        }
        else if (text.StartsWith("bottom", StringComparison.Ordinal))
        {
            read += 6;

            text = text[6..];

            if (text.StartsWith(" left", StringComparison.Ordinal))
            {
                read += 5;

                result = Left;
            }
            else if (text.StartsWith(" right", StringComparison.Ordinal))
            {
                read += 6;

                result = BottomRight;
            }
            else
            {
                result = Bottom;
            }
        }
        else if (text.StartsWith("left", StringComparison.Ordinal))
        {
            read += 4;

            result = Left;
        }
        else if (text.StartsWith("right", StringComparison.Ordinal))
        {
            read += 5;

            result = Right;
        }
        else
        {
            result = default;

            return false;
        }

        return true;
    }

    public static string Canonicalize(LinearGradientDirection value) => value switch
    {
        Bottom      => "bottom",
        BottomLeft  => "bottom left",
        BottomRight => "bottom right",
        Left        => "left",
        Right       => "right",
        Top         => "top",
        TopLeft     => "top left",
        TopRight    => "top right",
        _           => throw new Exception($"Unexpected direction: {value}")
    };
}