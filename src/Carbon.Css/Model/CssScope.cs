using System;
using System.Collections.Generic;

namespace Carbon.Css
{
    public class CssScope
    {
        private readonly IDictionary<string, CssValue> items;

        public CssScope(IDictionary<string, CssValue> items)
        {
            this.items = items;
        }

        public CssScope(CssScope parent = null)
        {
            Parent = parent;
            this.items = new Dictionary<string, CssValue>();
        }

        public object This { get; set; }

        public CssScope Parent { get; }

        public CssValue this[string name]
        {
            get => GetValue(name);
            set => items[name] = value;
        }

        public void Add(string name, CssValue value)
        {
            items.Add(name, value);
        }

        public bool TryGetValue(string key, out CssValue value)
        {
            return items.TryGetValue(key, out value);
        }

        public CssValue GetValue(string name, int counter = 0)
        {
            if (counter > 50) throw new Exception($"recussion detected: {counter}");

            if (items.TryGetValue(name, out CssValue value))
            {
                if (value.Kind == NodeKind.Variable)
                {
                    var variable = (CssVariable)value;

                    if (variable.Symbol == name) throw new Exception("Self referencing");

                    return GetValue(variable.Symbol, counter + 1);
                }

                return value;
            }

            if (Parent != null)
            {
                return Parent.GetValue(name, Count + 1);
            }
            else
            {
                return new CssUndefined(name);
            }
        }

        public int Count => items.Count;

        public void Clear()
        {
            items.Clear();
        }

        public CssScope GetChildScope() => new CssScope(this);
    }
}
