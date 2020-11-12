namespace Carbon.Css
{
    public sealed class CssUndefined : CssValue
    {
        public CssUndefined(string variableName)
            : base(NodeKind.Undefined)
        {
            VariableName = variableName;
        }

        public string VariableName { get; }

        public override CssNode CloneNode() => new CssUndefined(VariableName);

        public override string ToString() => $"/* ${VariableName} undefined */";
    }
}