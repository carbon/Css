namespace Carbon.Css.Tests;

public class NativeFunctionTests
{
    // 10.4. Trigonometric Functions: sin(), cos(), tan(), asin(), acos(), atan(), and atan2()

    [Theory]
    [InlineData("width: calc(sin(0.25turn) * 1s)")]
    [InlineData("width: calc(1rem * pow(1.5, -1))")]
    [InlineData("width: calc(100px * sqrt(9))")]
    [InlineData("width: calc(100px * tan(0.785398163rad))")]
    [InlineData("width: mod(1000px, 29rem)")]
    [InlineData("width: hypot(3px, 4px, 5px)")]
    [InlineData("transform: rotate(asin(2 * 0.125))", "transform: rotate(asin(0.25))")]
    [InlineData("transform: rotate(acos(2 * 0.125))", "transform: rotate(acos(0.25))")]
    [InlineData("width: abs(20% - 100px)")]
    [InlineData("line-height: rem(5.5, 2)")]
    [InlineData("margin-top: env(safe-area-inset-top, 20px)")]
    [InlineData("width: round(down, var(--height), var(--interval))")]
    [InlineData("grid-template-columns: fit-content(300px) fit-content(300px) 1fr")]
    [InlineData("offset-path: path(\"M 10 80 C 40 10, 65 10, 95 80 S 150 150, 180 80\")")]
    public void A(string declaration, string? expected = null)
    {
        var css = StyleSheet.Parse(
            $$"""
            div { 
              {{declaration}};
            }
            """);

        Assert.Equal(
            $$"""
            div {
              {{expected ?? declaration}};
            }
            """, css.ToString(), ignoreLineEndingDifferences: true);
    }
}
