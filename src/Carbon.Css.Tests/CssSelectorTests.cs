namespace Carbon.Css.Parser.Tests;

public class CssSelectorTests
{
    [Theory]
    [InlineData(":nth-child(1)")]
    [InlineData(":nth-child(-1)")]
    [InlineData(":nth-child(+1)")]
    public void NthChildTests(string text)
    {
        var selector = CssSelector.Parse(text);

        Assert.Equal(text, selector.ToString());
    }

    [Fact]
    public void ParseSelector()
    {
        var sheet = StyleSheet.Parse("div > h1 { width: 100px; }");

        var style = sheet.Children[0] as StyleRule;

        var selector = style.Selector;

        var a = (CssString)selector[0][0];
        var b = (CssString)selector[0][1];

        Assert.Equal("div", a.Text);
        Assert.Equal(" ", a.Trailing[0].Text);

        Assert.Equal(">", b.Text);
        Assert.Equal(" ", b.Trailing[0].Text);

        Assert.Single(sheet.Children);
        Assert.Equal(RuleType.Style, style.Type);
        Assert.Equal("div > h1", style.Selector.ToString());

        Assert.Single(style.Children);

        var x = (CssDeclaration)style[0];

        Assert.Equal("width", x.Name.ToString());
        Assert.Equal("100px", x.Value.ToString());
        Assert.Equal(
            """
            div > h1 {
              width: 100px;
            }
            """, sheet.ToString());
    }

    [Fact]
    public void A()
    {
        Assert.Equal("#a", CssSelector.Parse("#a").ToString());
        Assert.Equal(".a", CssSelector.Parse(".a").ToString());
    }

    [Fact]
    public void C()
    {
        Assert.Equal("#networkLinks .block .edit:before", CssSelector.Parse("#networkLinks .block .edit:before").ToString());
    }

    [Fact]
    public void MultiSelector()
    {
        var selector = CssSelector.Parse("h1, h2, h3");

        var h1 = selector[0];
        var h2 = selector[1];
        var h3 = selector[2];

        Assert.Equal("h1", h1[0].ToString());
        Assert.Equal("h2", h2[0].ToString());
        Assert.Equal("h3", h3[0].ToString());

        Assert.Null(h1[0].Trailing);
        Assert.Null(h2[0].Trailing);
        Assert.Null(h3[0].Trailing);

        Assert.Equal("h1, h2, h3", selector.ToString());
    }
}
