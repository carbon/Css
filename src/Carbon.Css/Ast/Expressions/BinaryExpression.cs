using Carbon.Css.Parser;

namespace Carbon.Css;

public sealed class BinaryExpression(
    CssValue left,
    CssToken op,
    CssValue right) : CssValue(NodeKind.Expression)
{
    public CssValue Left { get; } = left;

    public CssValue Right { get; } = right;

    public CssToken OperatorToken { get; } = op;

    public BinaryOperator Operator => (BinaryOperator)OperatorToken.Kind;

    public override BinaryExpression CloneNode() => new(Left, OperatorToken, Right);
}

// ||, &&, ==, !=
// +, -, *, /, %