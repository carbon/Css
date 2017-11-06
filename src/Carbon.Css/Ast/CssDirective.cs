using System;

namespace Carbon.Css
{
    public sealed class CssDirective : CssNode
    {
        public CssDirective(string name, string value)
            : base(NodeKind.Directive)
        {

            Name = name ?? throw new ArgumentNullException(nameof(name));
            Value = value;
        }

        public string Name { get; }

        public string Value { get; }
    }
}

/*
EXAMPLES:
//= support IE 11+
//= support Safari 5.1+
//= support Chrome 20+
*/
