using System.Collections.Generic;
using System.IO;

namespace Carbon.Css
{
    using Parser;

    // Single value
    public abstract class CssValue : CssNode
    {
        public CssValue(NodeKind kind)
            : base(kind)
        { }

        public static CssValue Parse(string text)
        {
            using (var reader = new SourceReader(new StringReader(text)))
            {
                var tokenizer = new CssTokenizer(reader, LexicalMode.Value);

                var parser = new CssParser(tokenizer);

                return parser.ReadValueList();
            }
        }

        public static CssValue FromComponents(IEnumerable<CssValue> components)
        {
            // A property value can have one or more components.
            // Components are seperated by a space & may include functions, literals, dimensions, etc

            var enumerator = components.GetEnumerator();

            enumerator.MoveNext();

            var first = enumerator.Current;

            if (!enumerator.MoveNext())
            {
                return first;
            }

            var list = new CssValueList(ValueSeperator.Space);

            list.Add(first);
            list.Add(enumerator.Current);

            while (enumerator.MoveNext())
            {
                list.Add(enumerator.Current);
            }

            return list;
        }
    }
}