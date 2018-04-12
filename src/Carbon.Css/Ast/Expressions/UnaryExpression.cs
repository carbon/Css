namespace Carbon.Css
{
    public sealed class UnaryExpression : CssValue
    {
        public UnaryExpression(UnaryOperator op, CssNode operand)
            : base(NodeKind.Expression)
        {
            Operand = operand;
            Operator = op;
        }

        public UnaryOperator Operator { get; }

        public CssNode Operand { get; }
    }
}