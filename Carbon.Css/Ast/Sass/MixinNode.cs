using System.Collections.Generic;

namespace Carbon.Css
{
    public class MixinNode : CssBlock
    {
        public MixinNode(string name, List<CssParameter> parameters)
            : base(NodeKind.Mixin)
        {
            Name = name;
            Parameters = parameters;
        }

        public string Name { get; }

        public List<CssParameter> Parameters { get; }
    }
}

/*
@mixin left($dist) {
  float: left;
  margin-left: $dist;
}
*/