using System.Collections.ObjectModel;
using System.Text;

namespace Carbon.Css
{
    using Parser;

    public class TokenList : Collection<CssToken>
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

        public override string ToString()
        {
            if (Count == 1)
            {
                return this[0].IsTrivia ? string.Empty : this[0].Text;
            }

            var sb = new StringBuilder();

            foreach (var token in this)
            {
                if (token.IsTrivia) continue;

                if (sb.Length != 0) sb.Append(" ");

                sb.Append(token.Text);
            }

            return sb.ToString();
        }
    }
}