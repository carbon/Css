using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Carbon.Css;

// A list of component values 

public sealed class CssValueList : CssValue, IReadOnlyList<CssValue>
{
    private readonly IList<CssValue> items;

    public CssValueList(IList<CssValue> values, CssValueSeperator seperator = CssValueSeperator.Comma)
       : base(NodeKind.ValueList)
    {
        this.items = values;
        Seperator = seperator;
    }

    public CssValueSeperator Seperator { get; }

    public CssValue this[int index] => items[index];

    public int Count => items.Count;

    public override CssNode CloneNode()
    {
        var clonedValues = new CssValue[items.Count];

        for (int i = 0; i < items.Count; i++)
        {
            clonedValues[i] = (CssValue)items[i].CloneNode();
        }

        return new CssValueList(clonedValues, Seperator);
    }

    internal override void WriteTo(TextWriter writer)
    {
        string seperator = Seperator == CssValueSeperator.Space ? " " : ", ";

        for (int i = 0; i < items.Count; i++)
        {
            if (i > 0)
            {
                writer.Write(seperator);
            }

            items[i].WriteTo(writer);
        }
    }

    public override string ToString()
    {
        return string.Join(Seperator == CssValueSeperator.Space ? " " : ", ", items);
    }

    #region IEnumerator

    IEnumerator<CssValue> IEnumerable<CssValue>.GetEnumerator() => items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => items.GetEnumerator();

    #endregion
}
