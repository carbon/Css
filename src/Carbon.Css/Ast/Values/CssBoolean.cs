namespace Carbon.Css
{
    public sealed class CssBoolean : CssValue
    {
        public static readonly CssBoolean True = new CssBoolean(true);
        public static readonly CssBoolean False = new CssBoolean(false);

        public CssBoolean(bool value)
            : base(NodeKind.Boolean)
        {
            Value = value;
        }

        public bool Value { get; }

        public override CssNode CloneNode() => new CssBoolean(Value);

        public override string ToString() => Value ? "true" : " false";

        internal static CssBoolean Get(bool value)
        {
            return value ? True : False;
        }
    }
}