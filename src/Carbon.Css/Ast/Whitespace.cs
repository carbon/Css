using System.Collections.ObjectModel;
using System.Text;

namespace Carbon.Css
{
    using Parser;

    public class Trivia : Collection<CssToken>
    {
        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var token in this)
            {
                sb.Append(token.ToString());
            }

            return sb.ToString();
        }
    }
}