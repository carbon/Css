using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Carbon.Css
{
    public sealed class CssSequence : CssValue, IEnumerable<CssValue>
    {
        private readonly List<CssValue> children;

        public CssSequence()
            : base(NodeKind.Sequence)
        {
            children = new List<CssValue>();
        }

        public CssSequence(int capacity)
            : base(NodeKind.Sequence)
        {
            children = new List<CssValue>(capacity);
        }

        public CssSequence(params CssValue[] items)
            : base(NodeKind.Sequence)
        {
            children = new List<CssValue>(items);
        }

        public void Add(CssValue item)
        {
            children.Add(item);
        }

        public void AddRange(IEnumerable<CssValue> items)
        {
            this.children.AddRange(items);
        }

        public int Count => children.Count;

        public CssValue this[int index] => children[index];

        public override string ToString()
        {
            var sb = StringBuilderCache.Aquire();

            using var writer = new StringWriter(sb);

            WriteTo(writer);

            return StringBuilderCache.ExtractAndRelease(sb);
        }
        
        public void WriteTo(TextWriter writer)
        {
            for (var i = 0; i < children.Count; i++)
            {
                var item = children[i];
                
                writer.Write(item.ToString());

                // Skip trailing trivia
                if ((item.Trailing != null || item.Kind == NodeKind.Sequence) && (i + 1) != children.Count)
                {
                    writer.Write(' ');
                }
            }
        }

        public bool Contains(NodeKind kind)
        {
            foreach (var token in children)
            {
                if (token.Kind == kind) return true;
            }

            return false;
        }

        public IEnumerator<CssValue> GetEnumerator()
        {
            return children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return children.GetEnumerator();
        }
    }
}