using System.Collections.Generic;
using System.IO;

namespace Carbon.Css
{
    public sealed class StyleRule : CssRule
    {
        public StyleRule(CssSelector selector)
        {
            Selector = selector;
        }

        public StyleRule(string selectorText)
            : this(CssSelector.Parse(selectorText)) { }

        public StyleRule(string selectorText, IReadOnlyList<CssNode> children)
            : this(CssSelector.Parse(selectorText))
        {
            foreach (var child in children)
            {
                child.Parent = this;

                base.Children.Add(child);
            }
        }

        public override RuleType Type => RuleType.Style;

        public CssSelector Selector { get; }

        public int Depth { get; set; }

        public override CssNode CloneNode()
        {
            var clone = new StyleRule(Selector) { Depth = Depth };

            foreach (var child in Children)
            {
                clone.Add(child.CloneNode());
            }

            return clone;
        }

        public override string ToString()
        {
            using (var sw = new StringWriter())
            {
                var writer = new CssWriter(sw);

                writer.WriteStyleRule(this, 0);

                return sw.ToString();
            }
        }

        public void WriteTo(TextWriter writer)
        {
            new CssWriter(writer).WriteStyleRule(this, 0);
        }

        #region Add Helper

        public void Add(string name, string value)
        {
            children.Add(new CssDeclaration(name, value));
        }

        #endregion
    }
}