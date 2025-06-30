using System.Text;

using Carbon.Css.Parser;

namespace Carbon.Css.Selectors;

internal sealed class SelectorList : List<Selector>
{
    public static SelectorList Parse(string text)
    {
        using var parser = new CssParser(text);

        return parser.ReadSelectorList();
    }

    public override string ToString()
    {
        var sb = new ValueStringBuilder(128);

        for (int i = 0; i < Count; i++)
        {
            if (i > 0)
            {
                sb.Append(", ");
            }

            this[i].WriteTo(ref sb);
        }

        return sb.ToString();
    }
}