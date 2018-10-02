using System;
using System.Text;

namespace Carbon.Css
{
    public sealed class CssDeclaration : CssNode
    {
        public CssDeclaration(string name, string value, string priority = null)
            : this(name, CssValue.Parse(value), priority)
        { }

        public CssDeclaration(string name, CssValue value, string priority = null)
            : base(NodeKind.Declaration)
        {
            if (name is null) throw new ArgumentNullException(nameof(name));

            Value = value ?? throw new ArgumentNullException(nameof(value));
            Priority = priority;
            Info = CssProperty.Get(name);
        }

        public CssDeclaration(string name, CssValue value, NodeKind kind)
            : base(kind)
        {
            Info = CssProperty.Get(name);
            Value = value;
        }

        public CssDeclaration(CssProperty property, CssValue value, string priority)
            : base(NodeKind.Declaration)
        {
            Info = property;
            Value = value;
            Priority = priority;
        }

        public string Name => Info.Name;

        public CssValue Value { get; }

        public CssProperty Info { get; }

        public string Priority { get; }

        public override CssNode CloneNode() => new CssDeclaration(Info, (CssValue)Value.CloneNode(), Priority);

        public override string ToString()
        {
            // color: red !important

            var sb = new StringBuilder(Info.Name);

            sb.Append(": ");
            sb.Append(Value.ToString());

            if (Priority != null)
            {
                sb.Append(" !").Append(Priority);
            }

            return sb.ToString();
        }
    }
}