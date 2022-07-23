using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

using Carbon.Css.Parser;

namespace Carbon.Css;

public sealed class Trivia : List<CssToken>
{
    [SkipLocalsInit]
    public override string ToString()
    {
        var sb = new ValueStringBuilder(stackalloc char[32]);

        foreach (var token in this)
        {
            sb.Append(token.Text);
        }

        return sb.ToString();
    }
}