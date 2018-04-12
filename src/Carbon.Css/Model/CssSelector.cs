using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace Carbon.Css
{
    using Parser;

    public readonly struct CssSelector : IEnumerable<string>
    {
        private readonly IList<string> items;

        public CssSelector(TokenList tokens)
        {
            var sb = new StringBuilder();

            var parts = new List<string>();

            foreach (var token in tokens)
            {
                if (token.Kind == TokenKind.Comma)
                {
                    parts.Add(sb.ToString().Trim());

                    sb.Clear();

                    continue;
                }

                if (token.IsTrivia)
                {
                    sb.Append(' '); // Prettify the trivia
                }
                else
                {
                    sb.Append(token.Text);
                }
            }

            if (sb.Length > 0)
            {
                parts.Add(sb.ToString().Trim());
            }

            this.items = parts;
        }

        public CssSelector(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            string[] parts = text.Split(Seperators.Comma);

            for (var i = 0; i < parts.Length; i++)
            {
                if (parts[i].IndexOf(' ') > -1)
                {
                    parts[i] = parts[i].Trim();
                }
            }
            
            this.items = parts;
        }

        public CssSelector(string[] items)
        {
            this.items = items;
        }

        public CssSelector(CssValue list)
        {
            if (list is CssValueList cssList)
            {
                var parts = new string[cssList.Count];

                for (var i = 0; i < cssList.Count; i++)
                {
                    parts[i] = cssList[i].ToString();
                }

                this.items = parts;
            }
            else
            {
                this.items = new[] { list.ToString() };
            }
        }

        public int Count => items.Count;

        public string this[int index] => items[index];

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
            if (items.Count == 1) return items[0];

            return string.Join(", ", items);
        }

        #region IEnumerator

        public IEnumerator<string> GetEnumerator() => items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => items.GetEnumerator();

        #endregion
    }
}

// a:hover
// #id
// .className
// .className, .anotherName			(Multiselector or group)