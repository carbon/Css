using System;

namespace Carbon.Css
{
    public abstract class CssNode
    {
        private CssNode parent;

        public CssNode(NodeKind kind, CssNode parent = null)
        {
            Kind = kind;

            this.parent = parent;
        }

        public NodeKind Kind { get; }

        public CssNode Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        internal Trivia Leading { get; set; }

        internal Trivia Trailing { get; set; }

        public virtual CssNode CloneNode()
        {
            throw new NotImplementedException();
        }
    }
}
