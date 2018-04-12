using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Carbon.Css
{
    // A list of component values 

    public sealed class CssValueList : CssValue, IReadOnlyList<CssValue>
    {
        private readonly IList<CssValue> items;

        public CssValueList(IList<CssValue> values, ValueSeperator seperator = ValueSeperator.Comma)
           : base(NodeKind.ValueList)
        {
            this.items = values;
            Seperator = seperator;
        }

        public ValueSeperator Seperator { get; }

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

        public override string ToString()
        {
            return string.Join(Seperator == ValueSeperator.Space ? " " : ", ", items.Select(t => t.ToString()));
        }

        #region IEnumerator

        IEnumerator<CssValue> IEnumerable<CssValue>.GetEnumerator() => items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => items.GetEnumerator();

        #endregion
    }

    public enum ValueSeperator
    {
        Comma,
        Space
    }
}