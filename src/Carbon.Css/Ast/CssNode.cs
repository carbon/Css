using System;
using System.Runtime.Serialization;

namespace Carbon.Css
{
    public abstract class CssNode
    {
        public CssNode(NodeKind kind, CssNode? parent = null)
        {
            Kind   = kind;
            Parent = parent;
        }

        [IgnoreDataMember]
        public NodeKind Kind { get; }

        [IgnoreDataMember]
        public CssNode? Parent { get; set; }

        [IgnoreDataMember]
        internal Trivia? Leading { get; set; }

        [IgnoreDataMember]
        public Trivia? Trailing { get; set; }

        public virtual CssNode CloneNode() => throw new NotImplementedException();
    }
}