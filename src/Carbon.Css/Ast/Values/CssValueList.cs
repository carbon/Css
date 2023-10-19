using System.Collections;
using System.IO;
using System.Text;

namespace Carbon.Css;

// A list of component values 

public sealed class CssValueList(
    IList<CssValue> values,
    CssValueSeparator separator = CssValueSeparator.Comma) : CssValue(NodeKind.ValueList), IReadOnlyList<CssValue>
{
    private readonly IList<CssValue> _items = values;

    public CssValueSeparator Separator { get; } = separator;

    public CssValue this[int index] => _items[index];

    public int Count => _items.Count;

    public override CssNode CloneNode()
    {
        var clonedValues = new CssValue[_items.Count];

        for (int i = 0; i < _items.Count; i++)
        {
            clonedValues[i] = (CssValue)_items[i].CloneNode();
        }

        return new CssValueList(clonedValues, Separator);
    }

    internal override void WriteTo(TextWriter writer)
    {
        string separator = Separator is CssValueSeparator.Space ? " " : ", ";

        for (int i = 0; i < _items.Count; i++)
        {
            if (i > 0)
            {
                writer.Write(separator);
            }

            _items[i].WriteTo(writer);
        }
    }

    internal override void WriteTo(scoped ref ValueStringBuilder sb)
    {
        string separator = Separator is CssValueSeparator.Space ? " " : ", ";

        for (int i = 0; i < _items.Count; i++)
        {
            if (i > 0)
            {
                sb.Append(separator);
            }

            _items[i].WriteTo(ref sb);
        }
    }

    public override string ToString()
    {
        return string.Join(Separator is CssValueSeparator.Space ? " " : ", ", _items);
    }

    #region IEnumerator

    IEnumerator<CssValue> IEnumerable<CssValue>.GetEnumerator() => _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();

    #endregion
}
