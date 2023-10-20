namespace Carbon.Css.Tests;

public class ForRuleTests
{
    [Fact]
    public void A_to()
    {
        var sheet = StyleSheet.Parse(
            """
            @for $i from 1 to 5 { 
              div { width: #{$i}px }
            }
            """);

        Assert.Equal(
            """
            div {
              width: 1px;
            }
            div {
              width: 2px;
            }
            div {
              width: 3px;
            }
            div {
              width: 4px;
            }
            """, sheet.ToString());
    }

    [Fact]
    public void A()
    {
        var sheet = StyleSheet.Parse(
            """
            @for $i from 1 through 5 { 
              div { width: #{$i}px }
            }
            """);

        Assert.Equal(
             """
            div {
              width: 1px;
            }
            div {
              width: 2px;
            }
            div {
              width: 3px;
            }
            div {
              width: 4px;
            }
            div {
              width: 5px;
            }
            """, sheet.ToString());
    }

    [Fact]
    public void B()
    {
        var sheet = StyleSheet.Parse("""
            @for $i from 1 through 5 { 
              .col-#{$i} { width: #{$i}px }
            }
            """);

        Assert.Equal("""
            .col-1 {
              width: 1px;
            }
            .col-2 {
              width: 2px;
            }
            .col-3 {
              width: 3px;
            }
            .col-4 {
              width: 4px;
            }
            .col-5 {
              width: 5px;
            }
            """, sheet.ToString());
    }

    [Fact]
    public void C()
    {
        var dic = new Dictionary<string, CssValue> {
            ["columnCount"] = CssValue.Parse("5"),
            ["columnWidth"] = CssValue.Number(100 / 5d),
            ["gap"] = CssValue.Parse("10px")
        };

        var sheet = StyleSheet.Parse("""
            @for $i from 1 through $columnCount { 
              .col-#{$i} { 
                left: #{$columnWidth * $i}%;
                margin: $gap * 0.5; 
              }
            }
            """);

        Assert.Equal("""
            .col-1 {
              left: 20%;
              margin: 5px;
            }
            .col-2 {
              left: 40%;
              margin: 5px;
            }
            .col-3 {
              left: 60%;
              margin: 5px;
            }
            .col-4 {
              left: 80%;
              margin: 5px;
            }
            .col-5 {
              left: 100%;
              margin: 5px;
            }
            """, sheet.ToString(dic));
    }
}
