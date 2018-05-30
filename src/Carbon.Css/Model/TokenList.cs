using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Carbon.Css
{
    using System.IO;
    using Parser;

    public sealed class TokenList : Collection<CssToken>
    {
        public string RawText
        {
            get
            {
                if (Count == 1)
                {
                    return this[0].Text;
                }

                var sb = new StringBuilder();

                foreach (var token in this)
                {
                    sb.Append(token.Text);
                }

                return sb.ToString();
            }
        }

        public void AddRange(IEnumerable<CssToken> tokens)
        {
            foreach (var token in tokens)
            {
                Add(token);
            }
        }

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

                    // Don't write back to back trivia
                    if (!this[i - 1].IsTrivia)
                    {
                        writer.Write(' ');
                    }

                    continue;
                }

                writer.Write(token.Text);
            }

        }

        public bool Contains(TokenKind kind)
        {
            foreach (var token in this)
            {
                if (token.Kind == kind) return true;
            }

            return false;
        }

        public TokenList Clone()
        {
            var list = new TokenList();

            foreach (var item in this)
            {
                list.Add(item);
            }

            return list;
        }

    }
}