namespace Carbon.Css.Tests;

public class StartingStyleRules
{
    [Fact]
    public void CanParseRoot()
    {
        var sheet = StyleSheet.Parse(
            """
            @starting-style {
              h1 {
                background-color: transparent;
              }
            }
            """);

        Assert.True(sheet.Children[0] is StartingStyleRule);

        Assert.Equal(
            """
            @starting-style {
              h1 {
                background-color: transparent;
              }
            }
            """, sheet.ToString());
    }

    [Fact]
    public void B()
    {
        var sheet = StyleSheet.Parse(
            """
            @starting-style {
              color: red;
              background-color: transparent;
            }
            """);

        Assert.True(sheet.Children[0] is StartingStyleRule);

        Assert.Equal(
            """
            @starting-style {
              color: red;
              background-color: transparent;
            }
            """, sheet.ToString());
    }

    [Fact]
    public void CanNest()
    {
        var sheet = StyleSheet.Parse(
            """
            div { 
              @starting-style {
                color: red;
                background-color: transparent;
              }
            }
            """, new CssContext { SupportsNesting = true });

        var style = (StyleRule)sheet.Children[0];

        Assert.True(style[0] is StartingStyleRule);

        Assert.Equal(
            """
            div {
              @starting-style {
                color: red;
                background-color: transparent;
              }
            }
            """, sheet.ToString());
    }
}