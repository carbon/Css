using System;
using System.Text.Json.Serialization;

namespace Carbon.Css.Gradients
{
    using static LinearGradientDirection;

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum LinearGradientDirection
    {
        None        = 0,
                    
        Top         = 1, // to top      | 0deg
        Bottom      = 2, // to bottom   | 180deg
        Left        = 3, // to left     | 270deg
        Right       = 4, // to right    | 90deg

        // Specify the corner the line goes toward
        // The angle is calculated on the aspect ratio of the containing box
        TopLeft     = 5,
        TopRight    = 6,
        BottomLeft  = 7,
        BottomRight = 8
    }

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

            if (text[0] == 't' && text[1] == 'o'  && text[2] == ' ') // to 
            {
                read += 3;

                text = text.Slice(3);
            }

            if (text.StartsWith("top".AsSpan()))
            {
                read += 3;

                text = text.Slice(3);

                if (text.StartsWith(" left".AsSpan()))
                {
                    read += 5;

                    result = TopLeft;
                }
                else if (text.StartsWith(" right".AsSpan()))
                {
                    read += 6;

                    result = TopRight;
                }
                else
                {
                    result = Top;
                }
            }
            else if (text.StartsWith("bottom".AsSpan()))
            {
                read += 6;

                text = text.Slice(6);

                if (text.StartsWith(" left".AsSpan()))
                {
                    read += 5;

                    result = Left;
                }
                else if (text.StartsWith(" right".AsSpan()))
                {
                    read += 6;

                    result = BottomRight;
                }
                else
                {
                    result = Bottom;
                }
            }
            else if (text.StartsWith("left".AsSpan()))
            {
                read += 4;

                result = Left;
            }
            else if (text.StartsWith("right".AsSpan()))
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
            _           => throw new Exception("Unexpected direction:" + value.ToString())
        };
    }

    // (top | bottom) (left | right)
}