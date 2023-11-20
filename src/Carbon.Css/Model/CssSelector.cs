using System.Collections;

using Carbon.Css.Parser;

namespace Carbon.Css;

public sealed class CssSelector(List<CssSequence> items) : IEnumerable<CssSequence>
{
    private readonly List<CssSequence> _items = items; // comma separated

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
// .className, .anotherName (multi-selector or group)