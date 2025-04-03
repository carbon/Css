using Carbon.Css.Parser;

namespace Carbon.Css.Selectors.Tests;

public class CssSelectorNewTests
{
    [Fact]
    public void CanParsePseudoClassSelector()
    {
        var parser = new CssParser(":nth-child(1)");

        var selector = parser.ReadNewSelector();

        Assert.Equal(CssSelectorType.PseudoClass, selector.Type);
        Assert.Equal(":nth-child", selector.Text);
        Assert.Equal("1", selector.Arguments.ToString());
    }

    [Fact]
    public void CanParseAttributeSelector()
    {
        var parser = new CssParser("[foo=\"bar\"]");

        var selector = parser.ReadNewSelector();

        Assert.Equal(CssSelectorType.Attribute, selector.Type);
        Assert.Equal("[foo=\"bar\"]", selector.Text);
    }

    [Fact]
    public void CanParseTagAndAttributeSelector()
    {
        var parser = new CssParser("a[foo=\"bar\"]");

        var selector = parser.ReadNewSelector();

        Assert.Equal(CssSelectorType.Tag, selector.Type);
        Assert.Equal("a", selector.Text);

        Assert.Equal(CssSelectorType.Attribute, selector.Next.Type);
        Assert.Equal("[foo=\"bar\"]", selector.Next.Text);

        Assert.Equal("a[foo=\"bar\"]", selector.ToString());
    }

    [Fact]
    public void CanParseIdSelector()
    {
        var parser = new CssParser("#post-1");

        var selector = parser.ReadNewSelector();

        Assert.Equal(CssSelectorType.Id, selector.Type);
        Assert.Equal("#post-1", selector.Text);
    }

    [Fact]
    public void CanParseIdSelector2()
    {
        var parser = new CssParser("#post-1 {");

        var selector = parser.ReadNewSelector();

        Assert.Equal(CssSelectorType.Id, selector.Type);
        Assert.Equal("#post-1", selector.Text);
    }

    [Fact]
    public void CanParseClassSelector()
    {
        var parser = new CssParser(".post");

        var selector = parser.ReadNewSelector();

        Assert.Equal(CssSelectorType.Class, selector.Type);
        Assert.Equal(".post", selector.Text);
    }

    [Fact]
    public void CanParseTagSelector()
    {
        var parser = new CssParser("carbon-image");

        var selector = parser.ReadNewSelector();

        Assert.Equal(CssSelectorType.Tag, selector.Type);
        Assert.Equal("carbon-image", selector.Text);
    }

    [Fact]
    public void CanParseCompoundSelector()
    {
        var parser = new CssParser(".post > :first-child");

        var selector = parser.ReadNewSelector();

        Assert.Equal(CssSelectorType.Class, selector.Type);
        Assert.Equal(".post", selector.Text);
    }

    [Fact]
    public void CanParseNextSiblingSelector()
    {
        var parser = new CssParser("p + img");

        var selector = parser.ReadNewSelector();

        Assert.True(selector is TagSelector);
        Assert.Equal(CombinatorType.AdjacentSibling, selector.Combinator);
        Assert.True(selector.Next is TagSelector);
    }

    [Fact]
    public void CanParseSubsequentSiblingSelector()
    {
        var parser = new CssParser("p ~ img");

        var selector = parser.ReadNewSelector();

        Assert.True(selector is TagSelector);
        Assert.Equal(CombinatorType.SubsequentSibling, selector.Combinator);
        Assert.True(selector.Next is TagSelector);

        Assert.Equal("p ~ img", selector.ToString());
    }

    [Fact]
    public void CanParseDescendantSelector()
    {
        var parser = new CssParser(".post span");

        var selector = parser.ReadNewSelector();

        Assert.Equal(CssSelectorType.Class, selector.Type);
        Assert.Equal(".post", selector.Text);

        Assert.Equal(CombinatorType.Descendant, selector.Combinator);

        Assert.Equal(CssSelectorType.Tag, selector.Next.Type);
        Assert.Equal("span", selector.Next.Text);

        Assert.Equal(".post span", selector.ToString());
    }

    [Fact]
    public void CanParseHas()
    {
        var parser = new CssParser(".post:has(.author)");

        var selector = parser.ReadNewSelector();

        Assert.Equal(CssSelectorType.Class, selector.Type);
        Assert.Equal(".post", selector.Text);
        Assert.Equal(default, selector.Combinator);
        Assert.Equal(":has", selector.Next?.Text);
    }
}
