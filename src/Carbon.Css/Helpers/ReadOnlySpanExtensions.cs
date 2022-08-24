using System.Globalization;

using Carbon.Color;

namespace Carbon.Css.Helpers;

internal static class ReadOnlySpanExtensions
{
    public static Rgba32 ReadColor(this ReadOnlySpan<char> text, out int read)
    {
        read = 0;

        char current;

        bool isFunction = false;

        while (read < text.Length)
        {
            current = text[read];

            // if we detect a function, read until )
            // otherwise, read until end or space

            if (isFunction)
            {
                if (current == ')')
                {
                    read++;
                    break;
                }
            }
            else if (current == '(')
            {
                isFunction = true;
            }
            else if (current is ' ' or ',')
            {
                break;
            }

            read++;
        }

        return Rgba32.Parse(text.Slice(0, read));
    }

    public static bool TryReadWhitespace(this ReadOnlySpan<char> text, out int read)
    {
        read = 0;

        while (text.Length > read && text[read] is ' ')
        {
            read++;
        }

        return read > 0;
    }
}
