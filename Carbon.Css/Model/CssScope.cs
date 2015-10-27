using System;
using System.Collections.Generic;

namespace Carbon.Css
{
    public class CssScope
    {
        private readonly CssScope parent;
        private readonly IDictionary<string, CssValue> items;

        private int counter = 0;

        public CssScope(IDictionary<string, CssValue> items)
        {
            this.items = items;
        }

        public CssScope(CssScope parent = null)
        {
            this.parent = parent;
            this.items = new Dictionary<string, CssValue>(); // StringComparer.OrdinalIgnoreCase
        }

        public object This { get; set; }

        public CssScope Parent => parent;

        public CssValue this[string name]
        {
            get { return GetValue(name); }
            set { items[name] = value; }
        }

        public void Add(string name, CssValue value)
        {
            items.Add(name, value);
        }

        public bool TryGetValue(string key, out CssValue value)
        {
            return items.TryGetValue(key, out value);
        }

        public CssValue GetValue(string name)
        {
            counter++;

            if (counter > 10000) throw new Exception("recussion detected");

            CssValue value;

            if (items.TryGetValue(name, out value))
            {
                if (value.Kind == NodeKind.Variable)
                {
                    var variable = (CssVariable)value;

                    if (variable.Symbol == name) throw new Exception("Self referencing");

                    return GetValue(variable.Symbol);
                }

                return value;
            }

            if (parent != null)
            {
                return parent.GetValue(name);
            }
            else
            {
                return new CssString($"/* ${name} not found */");
            }
        }

        public int Count => items.Count;

        public void Clear() => items.Clear();

        public CssScope GetChildScope()
        {
            return new CssScope(this);
        }
    }
}
