namespace Carbon.Css
{
    public class BinaryExpression : CssValue
    {
        // ||, &&, ==, !=
        // +, -, *, /, %

        public BinaryExpression(CssValue left, BinaryOperator op, CssValue right)
            : base(NodeKind.Expression)
        {
            Left = left;
            Operator = op;
            Right = right;
        }

        public CssValue Left { get; }

        public CssValue Right { get; }

        public BinaryOperator Operator { get; }

        public override CssNode CloneNode() => new BinaryExpression(Left, Operator, Right);
    }
}