using System;

namespace Carbon.Css.Helpers
{
    internal static class ReadOnlySpanExtensions
    {
        public static double ReadNumber(this ReadOnlySpan<char> text, out int read)
        {
            read = 0;

            while (char.IsDigit(text[read]) || text[read] == '.')
            {
                read++;
            }

            return double.Parse(text.Slice(0, read).ToString());
        }
    }
}
