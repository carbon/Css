using System.Collections.Generic;

namespace Carbon.Css
{
    public sealed class PageRule : CssRule
    {
        public PageRule(string selectorText, List<CssNode> children)
        {
            Selector = new CssSelector(selectorText);

            foreach (var child in children)
            {
                child.Parent = this;

                base.Children.Add(child);
            }
        }

        public override RuleType Type => RuleType.Page;

        public CssSelector Selector { get; }
    }
}