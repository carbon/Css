using System.Collections.ObjectModel;
using System.Text;

namespace Carbon.Css
{
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

        // public int Count => items.Count;

        // public CssToken this[int index] => items[index];

        public override string ToString()
        {
            if (Count == 1)
            {
                return this[0].IsTrivia ? string.Empty : this[0].Text;
            }

            var sb = new StringBuilder();

            int i = 0;

            foreach (var token in this)
            {
                if (i != 0 && token.IsTrivia)
                {
                    sb.Append(' ');

                    continue;
                }
                
                i++;

                sb.Append(token.Text);
            }

            return sb.ToString().Trim();
        }

        public bool Contains(string text)
        {
            foreach (var token in this)
            {
                if (token.Text.Contains(text)) return true;
            }

            return false;
        }
    }
}