namespace Carbon.Css.Tests;

public class DirectiveTests
{
    [Fact]
    public void ParsePartial()
    {
        var sheet = StyleSheet.Parse(
            """
            //= partial
            div { color: blue; }

            """); // Prevents standalone compilation

        Assert.Equal(
            """
            div {
              color: blue;
            }
            """, sheet.ToString());
    }
}