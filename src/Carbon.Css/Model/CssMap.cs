using System.Collections;

namespace Carbon.Css;

public sealed class CssMap : CssValue, IEnumerable<KeyValuePair<string, CssValue>>
{
    private readonly Dictionary<string, CssValue> _map = [];

    public CssMap()
        : base(NodeKind.Map) { }

    public void Add(string key, CssValue value)
    {
        _map.Add(key, value);
    }

    public CssValue this[string key] => _map[key];

    public int Count => _map.Count;

    public IEnumerator<KeyValuePair<string, CssValue>> GetEnumerator() => _map.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _map.GetEnumerator();
}
