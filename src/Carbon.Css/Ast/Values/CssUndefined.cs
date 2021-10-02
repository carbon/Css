namespace Carbon.Css;

public sealed class CssUndefined : CssValue
{
    public CssUndefined(string variableName)
        : base(NodeKind.Undefined)
    {
        VariableName = variableName;
    }

    public string VariableName { get; }

    public override CssUndefined CloneNode() => new(VariableName);

    public override string ToString() => $"/* ${VariableName} undefined */";
}