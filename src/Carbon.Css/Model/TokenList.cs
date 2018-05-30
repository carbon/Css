using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace Carbon.Css
{
    using Parser;

    public sealed class TokenList : Collection<CssToken>
    {
        public override string ToString()
        {
            using (var writer = new StringWriter())
            {
                WriteTo(writer);

                return writer.ToString();
            }
        }

        public void WriteTo(TextWriter writer)
        {
            if (Count == 1)
            {
                if (this[0].IsTrivia) return;

                writer.Write(this[0].Text);

                return;
            }

            for (var i = 0; i < Count; i++)
            {
                var token = this[i];

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