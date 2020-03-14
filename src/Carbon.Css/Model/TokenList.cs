using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace Carbon.Css
{
    using Parser;

    public sealed class TokenList : Collection<CssToken>
    {
        public override string ToString()
        {
            var sb = StringBuilderCache.Aquire();

            using var writer = new StringWriter(sb);

            WriteTo(writer);

            return StringBuilderCache.ExtractAndRelease(sb);
        }

        public void WriteTo(TextWriter writer)
        {
            if (Count == 1)
            {
                if (this[0].IsTrivia) return;

                writer.Write(this[0].Text);

                return;
            }

            for (int i = 0; i < Count; i++)
            {
                CssToken token = this[i];

                if (token.IsTrivia)
                {
                    // Skip leading and trailing trivia
                    if (i == 0 || i + 1 == Count) continue;

                    writer.Write(' ');

                    continue;
                }

                writer.Write(token.Text);
            }
        }
    }
}