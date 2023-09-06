namespace Carbon.Css;

public sealed class UnaryExpression(
    UnaryOperator op,
    CssNode operand) : CssValue(NodeKind.Expression)
{
    public UnaryOperator Operator { get; } = op;

    public CssNode Operand { get; } = operand;
}