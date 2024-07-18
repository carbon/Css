namespace Carbon.Css.Parser;

public sealed partial class CssParser : IDisposable
{
    public CssValue ReadExpression()
    {
        // Literal (Number, Measurement, Variable, ...
        var left = ReadComponent();

        return Current.IsBinaryOperator ? ReadExpressionFrom(left) : left;
    }

    public CssValue ReadExpressionFrom(CssValue left)
    {
        var operatorToken = Consume(); // Read operator

        ReadTrivia();

        // This may be another expression... 
        // TODO: Make recursive
        var right = ReadComponent();

        return new BinaryExpression(left, operatorToken, right);
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
