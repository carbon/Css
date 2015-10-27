using System;
using System.Text;

namespace Carbon.Css
{
    public class CssDeclaration : CssNode
    {
        private readonly CssProperty property;
        private readonly CssValue value;
        private readonly string priority;

        public CssDeclaration(string name, string value, string priority = null)
            : this(name, CssValue.Parse(value), priority)
        { }

        public CssDeclaration(string name, CssValue value, string priority = null)
            : base(NodeKind.Declaration)
        {
            #region Preconditions

            if (name == null) throw new ArgumentNullException("name");
            if (value == null) throw new ArgumentNullException("value");

            #endregion

            this.value = value;
            this.priority = priority;
            this.property = CssProperty.Get(name);
        }

        public CssDeclaration(string name, CssValue value, NodeKind kind)
            : base(kind)
        {
            this.property = CssProperty.Get(name);
            this.value = value;
        }

        public CssDeclaration(CssProperty property, CssValue value, string priority)
            : base(NodeKind.Declaration)
        {
            this.property = property;
            this.value = value;
            this.priority = priority;
        }

        public string Name => property.Name;

        public CssValue Value => value;

        public CssProperty Info => property;

        public string Priority => priority;

        public override CssNode CloneNode() => new CssDeclaration(property, (CssValue)value.CloneNode(), priority);

        public override string ToString()
        {
            // color: red !important

            var sb = new StringBuilder(property.Name);

            sb.Append(": ");
            sb.Append(value.ToString());

            if (priority != null)
            {
                sb.Append(" !").Append(priority);
            }

            return sb.ToString();
        }
    }
}