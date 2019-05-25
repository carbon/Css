using System.Collections;
using System.Collections.Generic;
using Carbon.Css.Parser;

namespace Carbon.Css
{
    public sealed class CssSelector : IEnumerable<CssSequence>
    {
        private readonly IReadOnlyList<CssSequence> items; // comma seperated

        public CssSelector(IReadOnlyList<CssSequence> items)
        {
            this.items = items;
        }      

        public int Count => items.Count;

        public CssSequence this[int index] => items[index];

        public bool Contains(NodeKind kind)
        {
            foreach (var part in items)
            {
                if (part.Contains(kind)) return true;
            }

            return false;
        }

        public override string ToString()
        {
            return string.Join(", ", items);
        }

        public static CssSelector Parse(string text)
        {
            using (var parser = new CssParser(text))
            {
                return parser.ReadSelector();
            }
        }

        #region IEnumerator

        public IEnumerator<CssSequence> GetEnumerator() => items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => items.GetEnumerator();

        #endregion
    }
}

// a:hover
// #id
// .className
// .className, .anotherName			(Multiselector or group)