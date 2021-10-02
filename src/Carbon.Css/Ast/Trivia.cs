using System.Collections.ObjectModel;
using System.Text;

using Carbon.Css.Parser;

namespace Carbon.Css;

public sealed class Trivia : Collection<CssToken>
{
    public override string ToString()
    {
        var sb = new ValueStringBuilder(32);

        foreach (var token in this)
        {
            sb.Append(token.Text);
        }

        return sb.ToString();
    }
}