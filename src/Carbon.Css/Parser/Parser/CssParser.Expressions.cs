using System;

namespace Carbon.Css.Parser
{
    public sealed partial class CssParser : IDisposable
    {
        public CssValue ReadExpression()
        {
            // Literal (Number, Measurement, Variable, ...
            var left = ReadComponent();

            // Check if there's a binary operator
            if (!current.IsBinaryOperator)
            {
                return left;
            }

            return ReadExpressionFrom(left);
        }

        public CssValue ReadExpressionFrom(CssValue left)
        {
            var opToken = Read(); // Read operator

            ReadTrivia();

            var op = (BinaryOperator)((int)opToken.Kind);

            // This may be another expression... 
            // TODO: Make recurssive
            var right = ReadComponent();

            return new BinaryExpression(left, op, right);
        }
    }
}

/*
// https://en.wikipedia.org/wiki/Shunting-yard_algorithm

 1: Multiplicative : *, /, %
 2: Additive       : +, – 
 3: ?			   : ==, !=, >, >=, <, <= 
 4: Logical        : &&, ||

 | number | plus | number | equals |  number  |
      5      +       10       ==       5
 |    BinaryExpression    |
 |               BinaryExpression             |
*/
