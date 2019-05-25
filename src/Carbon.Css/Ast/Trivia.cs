using System.Collections.ObjectModel;
using System.Text;

namespace Carbon.Css
{
    using Parser;

    public sealed class Trivia : Collection<CssToken>
    {
        public override string ToString()
        {
            var sb = StringBuilderCache.Aquire();

            foreach (var token in this)
            {
                sb.Append(token.ToString());
            }

            return StringBuilderCache.ExtractAndRelease(sb);
        }
    }
}