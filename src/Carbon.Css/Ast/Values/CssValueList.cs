using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Carbon.Css
{
    // Component Values 
    // Comma seperated list of a component values

    public class CssValueList : CssValue, IEnumerable<CssValue>
    {
        private readonly List<CssValue> items;
        private readonly ValueSeperator seperator;

        public CssValueList(ValueSeperator seperator = ValueSeperator.Comma)
            : this(new List<CssValue>(), seperator) { }

        public CssValueList(IEnumerable<CssValue> values, ValueSeperator seperator = ValueSeperator.Comma)
            : this(new List<CssValue>(values), seperator) { }

        public CssValueList(List<CssValue> values, ValueSeperator seperator = ValueSeperator.Comma)
           : base(NodeKind.ValueList)
        {
            this.items = values;
            this.seperator = seperator;
        }

        public ValueSeperator Seperator => seperator;

        public void Add(CssValue node) => items.Add(node);

        public CssValue this[int index] => items[index];

        public int Count => items.Count;

        public override CssNode CloneNode()
        {
            var clonedValues = new List<CssValue>();

            foreach (var item in items)
            {
                clonedValues.Add((CssValue)item.CloneNode());
            }

            return new CssValueList(clonedValues, seperator);
        }

        public override string ToString()
        {
            return string.Join(seperator == ValueSeperator.Space ? " " : ", ", items.Select(t => t.ToString()));
        }

        #region IEnumerator

        IEnumerator<CssValue> IEnumerable<CssValue>.GetEnumerator()
            => items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => items.GetEnumerator();

        #endregion
    }

    public enum ValueSeperator
    {
        Comma,
        Space
    }
}