using System;
using Carbon.Color;

namespace Carbon.Css.Helpers
{
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
                else if (current == ' ' || current == ',')
                {
                    break;
                }

                read++;
            }

            return Rgba32.Parse(text.Slice(0, read).ToString());           
        }

        public static double ReadNumber(this ReadOnlySpan<char> text, out int read)
        {
            read = 0;

            // leading -
            if (text[0] == '-')
            {
                read++;
            }

            while (text.Length > read && (char.IsDigit(text[read]) || text[read] == '.'))
            {
                read++;
            }

            return double.Parse(text.Slice(0, read).ToString());
        }

        public static bool TryReadWhitespace(this ReadOnlySpan<char> text, out int read)
        {
            read = 0;

            while (text.Length > read && text[read] == ' ')
            {
                read++;
            }

            return read > 0;
        }
    }
}
