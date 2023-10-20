using System.IO;

namespace Carbon.Css.Helpers;

public static class SourceHelper
{
    public static SourceLocation GetLocation(this string text, int position)
    {
        int i = 1;
        int line = 1;
        int column = 1;
        int charCode;

        using var reader = new StringReader(text);

        while ((charCode = reader.Read()) != -1)
        {
            var c = (char)charCode;

            if (c is '\n')
            {
                line++;
                column++;
            }

            i++;

            if (i == position) break;
        }

        return new SourceLocation(i, line, column);
    }

    public static List<LineInfo> GetLinesAround(ReadOnlySpan<char> text, int number, int window = 0)
    {
        var lines = new List<LineInfo>();
        var i = 1;

        foreach (var line in text.EnumerateLines())
        {
            if ((i >= (number - window)) && (i <= (number + window)))
            {
                lines.Add(new LineInfo(i, line.ToString()));
            }

            i++;

            if (i > (number + window)) break;
        }

        return lines;
    }
}