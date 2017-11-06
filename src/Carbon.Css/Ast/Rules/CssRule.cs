using System.IO;
using System.Text;

namespace Carbon.Css
{
    // A StyleRule rule has a selector, and one or more declarations

    public abstract class CssRule : CssBlock
    {
        public CssRule()
            : base(NodeKind.Rule)
        { }

        public abstract RuleType Type { get; }

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