namespace Carbon.Css
{
    public sealed class IncludeNode : CssNode
    {
        public IncludeNode(string name, CssValue? args)
            : base(NodeKind.Include)
        {
            Name = name;
            Args = args;
        }

        public string Name { get; }

        public CssValue? Args { get; }

        public override CssNode CloneNode() => new IncludeNode(Name, Args);
    }
}

// @include box-emboss(0.8, 0.05);