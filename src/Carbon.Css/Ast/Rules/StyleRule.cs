using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Carbon.Css
{
    public sealed class StyleRule : CssRule
    {
        private readonly CssSelector selector;

        public StyleRule(CssSelector selector)
        {
            this.selector = selector ?? throw new ArgumentNullException(nameof(selector));
        }

        public StyleRule(string selectorText)
            : this(new CssSelector(selectorText)) { }

        public StyleRule(string selectorText, List<CssNode> children)
            : this(new CssSelector(selectorText))
        {
            foreach (var child in children)
            {
                child.Parent = this;

                base.Children.Add(child);
            }
        }

        public override RuleType Type => RuleType.Style;

        public CssSelector Selector => selector;

        public override CssNode CloneNode()
        {
            var clone = new StyleRule(selector);

            foreach (var child in Children)
            {
                clone.Add(child.CloneNode());
            }

            return clone;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            using (var sw = new StringWriter(sb))
            {
                var writer = new CssWriter(sw);

                writer.WriteStyleRule(this, 0);

                return sb.ToString();
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