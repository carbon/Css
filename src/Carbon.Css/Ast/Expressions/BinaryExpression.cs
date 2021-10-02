using System.IO;

using Carbon.Css.Parser;

namespace Carbon.Css;

public sealed class BinaryExpression : CssValue
{
    // ||, &&, ==, !=
    // +, -, *, /, %

    public BinaryExpression(CssValue left, CssToken op, CssValue right)
        : base(NodeKind.Expression)
    {
        Left = left;
        OperatorToken = op;
        Right = right;
    }

    public CssValue Left { get; }

    public CssValue Right { get; }

    public CssToken OperatorToken { get; }

    public BinaryOperator Operator => (BinaryOperator)OperatorToken.Kind;

    public override BinaryExpression CloneNode() => new(Left, OperatorToken, Right);
}
