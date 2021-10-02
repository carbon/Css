using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Carbon.Css;

public sealed class CssScope
{
    private readonly IDictionary<string, CssValue> _items;

    public CssScope(IDictionary<string, CssValue> items)
    {
        _items = items;
    }

    public CssScope(CssScope? parent = null)
    {
        Parent = parent;
        _items = new Dictionary<string, CssValue>();
    }

    public object? This { get; set; }

    public CssScope? Parent { get; }

    public CssValue this[string name]
    {
        get => GetValue(name);
        set => _items[name] = value;
    }

    public void Add(string name, CssValue value)
    {
        _items.Add(name, value);
    }

    public bool TryGetValue(string key, [NotNullWhen(true)] out CssValue? value)
    {
        return _items.TryGetValue(key, out value);
    }

    public CssValue GetValue(string name, int counter = 0)
    {
        if (counter > 50)
        {
            throw new Exception($"recussion detected: {counter}");
        }

        if (_items.TryGetValue(name, out CssValue? value))
        {
            if (value.Kind == NodeKind.Variable)
            {
                var variable = (CssVariable)value;

                if (variable.Symbol == name) throw new Exception("Self referencing");

                return GetValue(variable.Symbol, counter + 1);
            }

            return value;
        }

        if (Parent is not null)
        {
            return Parent.GetValue(name, Count + 1);
        }
        else
        {
            return new CssUndefined(name);
        }
    }

    public int Count => _items.Count;

    public void Clear()
    {
        _items.Clear();
    }

    public CssScope GetChildScope() => new(this);
}
