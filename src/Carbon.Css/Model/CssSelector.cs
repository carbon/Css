using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Carbon.Css.Parser;

namespace Carbon.Css
{
    public readonly struct CssSelector : IEnumerable<TokenList>
    {
        private readonly IReadOnlyList<TokenList> items; // comma seperated

        public CssSelector(IReadOnlyList<TokenList> items)
        {
            this.items = items;
        }      

        public int Count => items.Count;

        public TokenList this[int index] => items[index];

        public bool Contains(string text)
        {
            if (items.Count == 1)
            {
                return items[0].Contains(text);
            }

            foreach (var part in items)
            {
                if (part.Contains(text)) return true;
            }

            return false;
        }

        public override string ToString()
        {
            if (items.Count == 1) return items[0].ToString();

            return string.Join(", ", items);
        }

        public static CssSelector Parse(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));
            
            string[] parts = text.Split(Seperators.Comma);

            var list = new TokenList[parts.Length];

            for (var i = 0; i < parts.Length; i++)
            {
                list[i] = new TokenList() { new CssToken(TokenKind.String, parts[i].Trim(), 0) };
            }

            return new CssSelector(list);
        }

        #region IEnumerator

        public IEnumerator<TokenList> GetEnumerator() => items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => items.GetEnumerator();

        #endregion
    }
}

// a:hover
// #id
// .className
// .className, .anotherName			(Multiselector or group)