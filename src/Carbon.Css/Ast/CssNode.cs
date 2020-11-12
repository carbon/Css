using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Carbon.Css
{
    public abstract class CssNode
    {
        public CssNode(NodeKind kind, CssNode? parent = null)
        {
            Kind   = kind;
            Parent = parent;
        }

        [JsonIgnore]
        public NodeKind Kind { get; }

        [JsonIgnore]
        public CssNode? Parent { get; set; }

        [JsonIgnore]
        internal Trivia? Leading { get; set; }

        [JsonIgnore]
        public Trivia? Trailing { get; set; }

        public virtual CssNode CloneNode()
        {
            throw new NotImplementedException(this.GetType().Name + " does not implement Clone");
        }
    }
}