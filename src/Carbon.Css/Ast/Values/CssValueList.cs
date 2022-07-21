using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Carbon.Css;

// A list of component values 

public sealed class CssValueList : CssValue, IReadOnlyList<CssValue>
{
    private readonly IList<CssValue> _items;

    public CssValueList(IList<CssValue> values, CssValueSeperator seperator = CssValueSeperator.Comma)
       : base(NodeKind.ValueList)
    {
        _items = values;
        Seperator = seperator;
    }

    public CssValueSeperator Seperator { get; }

    public CssValue this[int index] => _items[index];

    public int Count => _items.Count;

    public override CssNode CloneNode()
    {
        var clonedValues = new CssValue[_items.Count];

        for (int i = 0; i < _items.Count; i++)
        {
            clonedValues[i] = (CssValue)_items[i].CloneNode();
        }

        return new CssValueList(clonedValues, Seperator);
    }

    internal override void WriteTo(TextWriter writer)
    {
        string seperator = Seperator is CssValueSeperator.Space ? " " : ", ";

        for (int i = 0; i < _items.Count; i++)
        {
            if (i > 0)
            {
                writer.Write(seperator);
            }

            _items[i].WriteTo(writer);
        }
    }

    internal override void WriteTo(ref ValueStringBuilder sb)
    {
        string seperator = Seperator is CssValueSeperator.Space ? " " : ", ";

        for (int i = 0; i < _items.Count; i++)
        {
            if (i > 0)
            {
                sb.Append(seperator);
            }

            _items[i].WriteTo(ref sb);
        }
    }

    public override string ToString()
    {
        return string.Join(Seperator is CssValueSeperator.Space ? " " : ", ", _items);
    }

    #region IEnumerator

    IEnumerator<CssValue> IEnumerable<CssValue>.GetEnumerator() => _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();

    #endregion
}
