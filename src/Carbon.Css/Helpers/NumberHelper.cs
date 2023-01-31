using System.Buffers.Text;
using System.Globalization;

namespace Carbon.Css.Helpers;

internal static class NumberHelper
{
    public static double ReadNumber(ReadOnlySpan<char> text, out int read)
    {
        read = 0;

        // leading -
        if (text[0] is '-')
        {
            read++;
        }

        while (text.Length > read && (char.IsAsciiDigit(text[read]) || text[read] is '.'))
        {
            read++;
        }

        return double.Parse(text[..read], provider: CultureInfo.InvariantCulture);
    }

    public static double ReadNumber(ReadOnlySpan<byte> text, out int read)
    {
        read = 0;

        // leading -
        if (text[0] is (byte)'-')
        {
            read++;
        }

        // 0-9 or .
        while (text.Length > read && text[read] is (>= 48 and <= 57) or 46)
        {
            read++;
        }

        _ = Utf8Parser.TryParse(text[..read], out double value, out _);

        return value;
    }
}
