namespace Carbon.Css;

public sealed class CssUndefined(string variableName) 
    : CssValue(NodeKind.Undefined)
{
    public string VariableName { get; } = variableName;

    public override CssUndefined CloneNode() => new(VariableName);

    public override string ToString() => $"/* ${VariableName} undefined */";
}