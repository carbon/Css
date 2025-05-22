namespace Carbon.Css.Tests;

public class ColorTests
{
    [Fact]
    public void RgbaTest()
    {
        var sheet = StyleSheet.Parse("div { color: rgba(100, 100, 100, 0.5); }");

        Assert.Equal(
            """
            div {
              color: rgba(100, 100, 100, 0.5);
            }
            """, sheet.ToString());
    }

    [Fact]
    public void Q()
    {
        var text = 
            """
            div {
              background-color: rgba(32, 185, 235, 0.05);
            }
            """;

        var sheet = StyleSheet.Parse(text);

        Assert.Equal(text, sheet.ToString());
    }

    [Fact]
    public void CanUseColorMix()
    {
        var sheet = StyleSheet.Parse("div { color: color-mix(in lch, purple 50%, plum 50%); }");

        Assert.Equal(
            """
            div {
              color: color-mix(in lch, purple 50%, plum 50%);
            }
            """, sheet.ToString(), ignoreWhiteSpaceDifferences: true);
    }

    [Fact]
    public void Rgba1()
    {
        var sheet = StyleSheet.Parse("div { color: rgb(255, 255, 255, .5); }");

        Assert.Equal(
            """
            div {
              color: rgb(255, 255, 255, 0.5);
            }
            """, sheet.ToString());
    }

    [Fact]
    public void Rgba2()
    {
        var sheet = StyleSheet.Parse("div { color: rgb(255 255 255 / 50%); }");

        Assert.Equal(
            """
            div {
              color: rgb(255 255 255 / 50%);
            }
            """, sheet.ToString());
    }

    [Fact]
    public void HexA()
    {
        Assert.Equal("red", CssValue.Parse("red").ToString());

        var value = StyleSheet.Parse("body { background-color: rgba(#ffffff, 0.5) }");

        Assert.Equal(
            """
            body {
              background-color: #ffffff80;
            }
            """, value.ToString());
    }

    [Fact]
    public void Rgba3()
    {
        var sheet = StyleSheet.Parse("div { color: rgba(#748297, .31); }");

        Assert.Equal(
            """
            div {
              color: #7482974f;
            }
            """, sheet.ToString());
    }


    [Fact]
    public void Hsl1()
    {
        var sheet = StyleSheet.Parse("div { color: hsl(11, 100%, 50%); }");

        Assert.Equal(
            """
            div {
              color: hsl(11, 100%, 50%);
            }
            """, sheet.ToString());
    }

    [Fact]
    public void Rgba4()
    {
        var s1 = StyleSheet.Parse("div { color: rgba(#748297, 30%); }");
        var s2 = StyleSheet.Parse("div { color: rgba(#748297, 0.3); }");

        var expected =
            """
            div {
              color: #7482974c;
            }
            """;

        Assert.Equal(expected, s1.ToString());
        Assert.Equal(expected, s2.ToString());
    }

    [Fact]
    public void LinearGradient()
    {
        var s1 = StyleSheet.Parse("div { background: linear-gradient(0deg, rgb(29 30 40 / 0%) 0%, red 94%); }");

        Assert.Equal(
            """
            div {
              background: linear-gradient(0deg, rgb(29 30 40 / 0%) 0%, red 94%);
            }
            """, s1.ToString());
    }

    [Fact]
    public void NestedRgb()
    {
        var s1 = StyleSheet.Parse("div { color: rgba(rgb(2 3 4), 30%); }");

        Assert.Equal(
            """
            div {
              color: #0203044c;
            }
            """, s1.ToString());
    }
}

// https://www.bram.us/2020/04/27/colors-in-css-hello-space-separated-functional-color-notations/