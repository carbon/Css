namespace Carbon.Css.Tests;

public class MaskingTests
{
    [Fact]
    public void A()
    {
        var sheet = StyleSheet.Parse(
            """
            //= support Safari >= 5

            div {
                mask-image: url('mask.svg');
            }
            """);

        Assert.Equal(
            """
            div {
              -webkit-mask-image: url('mask.svg');
              mask-image: url('mask.svg');
            }
            """, sheet.ToString(), ignoreLineEndingDifferences: true);
    }
}