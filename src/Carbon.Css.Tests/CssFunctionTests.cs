namespace Carbon.Css.Tests;

public class CssFunctionTests
{
    [Fact]
    public void Unquote()
    {
        var sheet = StyleSheet.Parse(
          """
          $t1: "calc((100% - 600px) / 2)"; 

          div { padding: unquote($t1) }     
          """);

        Assert.Equal("div { padding: calc((100% - 600px) / 2); }", sheet.ToString());
    }

    [Fact]
    public void ParseFunctionWithLeadingWhitespaceInArgument()
    {
        var sheet = StyleSheet.Parse("div { background-color: rgba( 42, 45, 53, 0.7); }");

        Assert.Equal("div { background-color: rgba(42, 45, 53, 0.7); }", sheet.ToString());
    }
}