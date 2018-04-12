namespace Carbon.Css
{
    public sealed class CssBoolean : CssValue
    {
        public CssBoolean(bool value)
            : base(NodeKind.Boolean)
        {
            Value = value;
        }

        public bool Value { get; }

        public override CssNode CloneNode() => new CssBoolean(Value);

        public override string ToString() => Value.ToString().ToLower();
    }
}