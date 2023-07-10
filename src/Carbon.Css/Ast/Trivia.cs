using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

using Carbon.Css.Parser;

namespace Carbon.Css;

public sealed class Trivia : List<CssToken>
{
    [SkipLocalsInit]
    public override string ToString()
    {
        var sb = new ValueStringBuilder(stackalloc char[32]);

        foreach (ref readonly CssToken token in CollectionsMarshal.AsSpan(this))
        {
            sb.Append(token.Text);
        }

        return sb.ToString();
    }
}