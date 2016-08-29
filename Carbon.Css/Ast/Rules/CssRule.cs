using System.IO;
using System.Text;

namespace Carbon.Css
{
    // A StyleRule rule has a selector, and one or more declarations

    public class CssRule : CssBlock
    {
        public CssRule(RuleType type)
            : this(type, NodeKind.Rule)
        { }

        public CssRule(RuleType type, NodeKind kind)
            : base(kind)
        {
            Type = type;
        }

        public RuleType Type { get; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            using (var sw = new StringWriter(sb))
            {
                new CssWriter(sw);

                return sb.ToString();
            }
        }
    }
}