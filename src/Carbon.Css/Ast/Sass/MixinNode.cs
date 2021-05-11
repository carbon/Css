using System.Collections.Generic;

namespace Carbon.Css
{
    public sealed class MixinNode : CssBlock
    {
        public MixinNode(string name, IReadOnlyList<CssParameter> parameters)
            : base(NodeKind.Mixin)
        {
            Name = name;
            Parameters = parameters;
        }

        public string Name { get; }

        public IReadOnlyList<CssParameter> Parameters { get; }
    }
}

/*
@mixin left($dist) {
  float: left;
  margin-left: $dist;
}
*/
