namespace Carbon.Css.Tests;

public class OperatorTests
{
    [Fact]
    public void AreCompatible()
    {
        Assert.False(CssValue.AreCompatible(CssValue.Parse("93.75%"), CssValue.Parse("65px"), BinaryOperator.Subtract));
        Assert.False(CssValue.AreCompatible(CssValue.Parse("93.75%"), CssValue.Parse("65px"), BinaryOperator.Add));
        Assert.True(CssValue.AreCompatible(CssValue.Parse("93.75%"), CssValue.Parse("65px"), BinaryOperator.Multiply));
        // Assert.True(CssValue.AreCompatible(CssValue.Parse("93.75%"), CssValue.Parse("65px"), BinaryOperator.Divide));
    }

    // width: 600px / 960px * 100%;

    [Fact]
    public void Divide()
    {
        var expression = (CssValue.Parse("100% / 3") as BinaryExpression);

        Assert.Equal(BinaryOperator.Divide, expression.Operator);
    }

    /*
    [Fact]
    public void DivideInParenthesis()
    {
        var expression = (CssValue.Parse("(100% / 3)") as BinaryExpression);

        Assert.Equal(BinaryOperator.Divide, expression.Operator);
    }
    */

    [Fact]
    public void Calc()
    {
        var a = CssValue.Parse("calc(93.75% - 65px)") as CssFunction;
        var b = CssValue.Parse("calc(100% - 80px)") as CssFunction;
    }

    [Fact]
    public void ParseSubtract()
    {
        var expression = (BinaryExpression)CssValue.Parse("93.75% - 65px");

        Assert.Equal(new CssUnitValue(93.75, "%"), expression.Left);
        Assert.Equal(new CssUnitValue(65, "px"), expression.Right);

        Assert.Equal(BinaryOperator.Subtract, expression.Operator);
    }

    [Fact]
    public void FontSyntaxIsOk()
    {
        // https://stackoverflow.com/questions/4988944/how-to-prevent-division-when-using-variables-separated-by-a-slash-in-css-propert

        // font-size/line-height
        var css = StyleSheet.Parse("""
            div {
                font: italic small-caps normal 13px/150% Arial, Helvetica, sans-serif;
            }
            """);

        Assert.Equal("""
            div { font: italic small-caps normal 13px / 150% Arial, Helvetica, sans-serif; }
            """, css.ToString());
    }

    [Fact]
    public void A()
    {
        var css = StyleSheet.Parse("""
            div {
                width: 10px / 2;
            }
            """);

        var div = (StyleRule)css.Children[0];

        var width = div.GetDeclaration("width");

        var a = width.Value as BinaryExpression;

        Assert.Equal(BinaryOperator.Divide, a.Operator);
    }

    [Fact]
    public void ExpressionTest7()
    {
        var css = StyleSheet.Parse(
            """
            $bgColor: #ffffff;

            @if rgba($bgColor, 0.5) == rgba(255, 255, 255, 0.5) { 
              div {
                color: darken($bgColor, 50%);
                background-color: darken($bgColor, 0.5);
              }
            }
            """);

        Assert.Equal(
            """
            div {
              color: #808080;
              background-color: #808080;
            }
            """, css.ToString());
    }

    [Fact]
    public void ExpressionTest1()
    {
        var sheet = StyleSheet.Parse(
            """
            $bgColor: orange;

            @if $bgColor == orange { 
              div {
                background-color: $bgColor;
              }
            }
            """);

        Assert.Equal("div { background-color: orange; }", sheet.ToString().Trim());
    }

    [Fact]
    public void ExpressionTest2()
    {
        var sheet = StyleSheet.Parse(
            """
            $bgColor: #ffffff;

            @if rgba($bgColor, 0.5) == rgba(255, 255, 255, 0.5) { 
              div {
                background-color: $bgColor;
              }
            }
            """);

        Assert.Equal("div { background-color: #ffffff; }", sheet.ToString());
    }

    [Fact]
    public void ExpressionTest5()
    {
        var sheet = StyleSheet.Parse(
            """
            $bgColor: #ffffff;

            @if rgba($bgColor, 0.5) == rgba(255, 255, 255, 0.5) { 
              $bgColor: purple;  
            }

            div {
              background-color: $bgColor;
            }
            """);

        Assert.Equal("div { background-color: purple; }", sheet.ToString());
    }

    [Fact]
    public void IfTest7()
    {
        var sheet = StyleSheet.Parse("div { color: if(red == red, purple, green) }");

        Assert.Equal("div { color: purple; }", sheet.ToString());
    }

    [Fact]
    public void IfTest8()
    {
        var sheet = StyleSheet.Parse("div { color: if(red == orange, purple, green) }");

        Assert.Equal("div { color: green; }", sheet.ToString());
    }

    [Fact]
    public void IfTest4()
    {
        var sheet = StyleSheet.Parse("""
            $bgColor: #ffffff;

            @if rgba($bgColor, 0.5) == rgba(255, 255, 255, 0.5) { 
              div { color: red; display: none; }
              div { background-color: orange; }
            }

            """);

        Assert.Equal(
            """
            div {
              color: red;
              display: none;
            }
            div { background-color: orange; }
            """, sheet.ToString());
    }
}