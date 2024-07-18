namespace Carbon.Css.Tests;

public class PseudoClassFunctionTests
{
    [Fact]
    public void Q1()
    {
        var css = StyleSheet.Parse(
            """
            body:has(mymind-lightbox > .intro-video) {
              mymind-lightbox-close-bar {
               display: none !important;
              }
            }
            """);

        var rule = (StyleRule)css.Children[0];

        Assert.Single(rule.Selector);

        var selector = rule.Selector[0];

        Assert.Equal(
            """
            body:has(mymind-lightbox > .intro-video) mymind-lightbox-close-bar {
              display: none !important;
            }
            """, css.ToString());
    }

    [Fact]
    public void Q()
    {
        var css = StyleSheet.Parse(
            """
            body:has(mymind-lightbox-close-bar:hover) {
              mymind-backdrop {
                opacity: 0.8 !important;
              }  
            }
            """);


        Assert.Equal(
            """
            body:has(mymind-lightbox-close-bar:hover) mymind-backdrop {
              opacity: 0.8 !important;
            }
            """, css.ToString());
    }

    [Theory]
    [InlineData(".irrelevant")]
    [InlineData(".notice.alert")]
    [InlineData(".notice.alert span")]
    [InlineData(".irrelevant > span")]
    [InlineData(".irrelevant > [name=\"test\"] ~ turtles")]
    public void NotTests(string selector)
    {
        var css = StyleSheet.Parse(
           $$"""
           p:not({{selector}}) {
             font-weight: bold;
           }
           """);

        Assert.Equal(
            $$"""
            p:not({{selector}}) {
              font-weight: bold;
            }
            """, css.ToString());
    }

   
    [Fact]
    public void WhereTests()
    {
        var css = StyleSheet.Parse(
           """
           :where(ol, ul, menu:unsupported) :where(ol, ul) {
             color: green;
           }
           """);


        Assert.Equal(
            """
            :where(ol, ul, menu:unsupported) :where(ol, ul) {
              color: green;
            }
            """, css.ToString());
    }

    [Fact]
    public void A()
    {
        var css = StyleSheet.Parse(
            """
            :is(button, .custom-input) {
              border-radius: 4px;
              padding: 8px 12px; 
            }
            """);

        Assert.Equal(
            """
            :is(button, .custom-input) {
              border-radius: 4px;
              padding: 8px 12px;
            }
            """, css.ToString());
    }

    [Fact]
    public void CanNest()
    {
        var css = StyleSheet.Parse(
            """
            div {
              :is(button, .custom-input) {
                border-radius: 4px;
                padding: 8px 12px; 
              }
            }
            """);

        Assert.Equal(
            """
            div :is(button, .custom-input) {
              border-radius: 4px;
              padding: 8px 12px;
            }
            """, css.ToString());
    }

    [Fact]
    public void B()
    {
        var sheet = StyleSheet.Parse(
            """
            :is(section, article, aside, nav) h1 {
              font-size: 25px;
            }
            """, new CssContext());

        var rule = (StyleRule)sheet.Children[0];

        Assert.Single(rule.Selector);

        Assert.Equal(":is(section, article, aside, nav) h1", rule.Selector.ToString());

        var s = rule.Selector[0];

        Assert.Equal(2, s.Count);
        var func = (CssFunction)s[0];
        Assert.True(func.Arguments is CssValueList);
        Assert.Equal(":is(section, article, aside, nav)", s[0].ToString());

        Assert.Equal(
            """
            :is(section, article, aside, nav) h1 {
              font-size: 25px;
            }
            """, sheet.ToString());
    }
}
