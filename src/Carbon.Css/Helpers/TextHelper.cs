using System.Collections.Generic;
using System.IO;

namespace Carbon.Css.Helpers
{
    // Move to SourceReader

    public static class TextHelper
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

                if (c == '\n')
                {
                    line++;
                    column++;
                }

                i++;

                if (i == position) break;
            }

            return new SourceLocation(i, line, column);
        }

        public static IEnumerable<LineInfo> GetLinesAround(this string text, int number, int window = 0)
        {
            using var reader = new StringReader(text);

            var i = 1;
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                if ((i >= (number - window)) && (i <= (number + window)))
                {
                    yield return new LineInfo(i, line);
                }

                i++;

                if (i > (number + window)) break;
            }
        }
    }
}