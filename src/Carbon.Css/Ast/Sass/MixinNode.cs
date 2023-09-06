namespace Carbon.Css;

public sealed class MixinNode(
    string name,
    IReadOnlyList<CssParameter> parameters) : CssBlock(NodeKind.Mixin)
{
    public string Name { get; } = name;

    public IReadOnlyList<CssParameter> Parameters { get; } = parameters;
}

/*
@mixin left($dist) {
  float: left;
  margin-left: $dist;
}
*/
