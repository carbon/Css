using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Carbon.Css
{
    // Component Values 
    // Comma seperated list of a component values

    public class CssValueList : CssValue, IEnumerable<CssValue>
    {
        private readonly List<CssValue> items = new List<CssValue>();

        private readonly ValueSeperator seperator;

        public CssValueList(ValueSeperator seperator = ValueSeperator.Comma)
            : base(NodeKind.ValueList)
        {
            this.seperator = seperator;
        }

        public CssValueList(IEnumerable<CssValue> values, ValueSeperator seperator = ValueSeperator.Comma)
            : base(NodeKind.ValueList)
        {
            this.items.AddRange(values);

            this.seperator = seperator;
        }

        public ValueSeperator Seperator => seperator;

        public void Add(CssValue node) => items.Add(node);

        public CssValue this[int index] => items[index];

        public int Count => items.Count;

        public override CssNode CloneNode() => new CssValueList(items.Select(c => (CssValue)c.CloneNode()), seperator);

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