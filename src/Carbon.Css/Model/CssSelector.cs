using System.Collections;
using System.Collections.Generic;
using Carbon.Css.Parser;

namespace Carbon.Css;

public sealed class CssSelector : IEnumerable<CssSequence>
{
    private readonly IReadOnlyList<CssSequence> _items; // comma seperated

    public CssSelector(IReadOnlyList<CssSequence> items)
    {
        _items = items;
    }

    public int Count => _items.Count;

    public CssSequence this[int index] => _items[index];

    public bool Contains(NodeKind kind)
    {
        foreach (var part in _items)
        {
            if (part.Contains(kind)) return true;
        }

        return false;
    }

    public override string ToString() => string.Join(", ", _items);

    public static CssSelector Parse(string text)
    {
        using var parser = new CssParser(text);

        return parser.ReadSelector();
    }

    #region IEnumerator

    public IEnumerator<CssSequence> GetEnumerator() => _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();

    #endregion
}

// a:hover
// #id
// .className
// .className, .anotherName			(Multiselector or group)