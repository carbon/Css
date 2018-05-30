using System;

namespace Carbon.Css
{
    public abstract class CssNode
    {
        public CssNode(NodeKind kind, CssNode parent = null)
        {
            Kind   = kind;
            Parent = parent;
        }

        public NodeKind Kind { get; }

        public CssNode Parent { get; set; }

        internal Trivia Leading { get; set; }

        public Trivia Trailing { get; set; }

        public virtual CssNode CloneNode()
        {
            throw new NotImplementedException();
        }
    }
}
