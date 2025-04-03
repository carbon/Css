
using Carbon.Css.Selectors;

namespace Carbon.Css.Parser.Tests;

public class CssSelectorListTests
{
    [Fact]
    public void CanParse()
    {
        var selector = SelectorList.Parse("h1, h2, h3");

        var h1 = selector[0];
        var h2 = selector[1];
        var h3 = selector[2];

        Assert.Equal("h1", h1.ToString());
        Assert.Equal("h2", h2.ToString());
        Assert.Equal("h3", h3.ToString());

        Assert.Equal("h1, h2, h3", selector.ToString());
    }
}