namespace Carbon.Css.Tests;

public class NestingTests
{
    [Fact]
    public void SingleNestedReference()
    {
        var css = StyleSheet.Parse("div { &.hide { display: none; } }");

        Assert.Equal(
            """
            div.hide {
              display: none;
            }
            """, css.ToString(), ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void SingleDoubleNestedReference()
    {
        var css = StyleSheet.Parse(
            """
            #networkLinks .block {
              .edit {
                opacity: 0;
                
                &:before {
                  font-family: 'carbon';
                }
              }
            }
            """);

        var node = (((css.Children[0] as StyleRule).Children[0] as StyleRule)).Children[1] as StyleRule;

        var selector = CssWriter.ExpandSelector(node);

        Assert.True(selector[0][0] is CssReference);
        Assert.True(selector[0][1] is CssString);

        // throw new System.Exception(string.Join(Environment.NewLine, selector[0].Select(a => a.Kind + " " + a.ToString())));

        Assert.Equal(
            """
            #networkLinks .block .edit:before {
              font-family: 'carbon';
            }
            #networkLinks .block .edit {
              opacity: 0;
            }
            """, css.ToString(), ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void NestedSelectorList()
    {
        var css = StyleSheet.Parse(
            """
            div {
              input,
              textarea {
                display: block;
              } 
            }
            """);

        Assert.Equal(
            """
            div input,
            div textarea {
              display: block;
            }
            """, css.ToString(), ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void SelectorListWithNestedReference()
    {
        var css = StyleSheet.Parse("div, span { .hide { display: none; } }");

        Assert.Equal(
            """
            div .hide,
            span .hide {
              display: none;
            }
            """, css.ToString());
    }

    [Fact]
    public void B()
    {
        var css = StyleSheet.Parse("div { &.hide, &.hidden { display: none; } }");

        Assert.Equal(
            """
            div.hide,
            div.hidden {
              display: none;
            }
            """, css.ToString(), ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void C()
    {
        var css = StyleSheet.Parse(".hide { body & { display: none; } }");

        Assert.Equal(
            """
            body .hide {
              display: none;
            }
            """, css.ToString(), ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void NestedMultiselector1()
    {
        var css = StyleSheet.Parse(
            """
            .details {
              max-width: 60rem;

              .description {
                ul {
                  list-style: disc;
                }
                p, ul, ol {
                  font-size: 1.2em;
                
                  &:last-child {
                    margin-bottom: 0;
                  }
                }
              }
            }
            """);


        Assert.Equal("""
            .details {
              max-width: 60rem;
            }
            .details .description ul {
              list-style: disc;
            }
            .details .description p:last-child,
            .details .description ul:last-child,
            .details .description ol:last-child {
              margin-bottom: 0;
            }
            .details .description p,
            .details .description ul,
            .details .description ol {
              font-size: 1.2em;
            }
            """, css.ToString(), ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void NestedStyleRewriterTest()
    {
        var sheet = StyleSheet.Parse("""
            nav {
              display: block;
              ul {
                margin: 0;
                padding: 0;
                list-style: none;
              }

              li { display: inline-block; }

              a {
                display: block;
                padding: 6px 12px;
                text-decoration: none;
              }
            }
            """);

        Assert.Equal(
            """
            nav {
              display: block;
            }
            nav ul {
              margin: 0;
              padding: 0;
              list-style: none;
            }
            nav li {
              display: inline-block;
            }
            nav a {
              display: block;
              padding: 6px 12px;
              text-decoration: none;
            }
            """, sheet.ToString(), ignoreLineEndingDifferences: true);
    }


    [Fact]
    public void NestedMultiSelector()
    {
        string text = 
            """
            #header { 
              min-height: 80px; 

              a {
                color: rgba(255,255,255,0.6);
              }

              header a {
                color: #fcfcfc;
              }

              .inner {
                display: table;
  
                h1, ul {
                  display: table-cell;
                }

                h1 {
                  font-size: 16px;
                }

                ul {
                  padding-left: 20px;

                  li {
                    display: inline-block;
                  }
                }
              }
            }
            """;

        Assert.Equal(
            """
            #header {
              min-height: 80px;
            }
            #header a {
              color: rgba(255, 255, 255, 0.6);
            }
            #header header a {
              color: #fcfcfc;
            }
            #header .inner h1,
            #header .inner ul {
              display: table-cell;
            }
            #header .inner h1 {
              font-size: 16px;
            }
            #header .inner ul li {
              display: inline-block;
            }
            #header .inner ul {
              padding-left: 20px;
            }
            #header .inner {
              display: table;
            }
            """, StyleSheet.Parse(text).ToString(), ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void NestedMultiSelector2()
    {
        var stylesheet = StyleSheet.Parse(
            """
            #header { 
              .inner {  
                h1, ul {
                  display: table-cell;
                  vertical-align: middle;
                }
              }
            }
            """);

        var node = (StyleRule)stylesheet.Children[0];

        Assert.Equal(1, node.Depth);

        node = (StyleRule)node.Children[0]; // .inner

        Assert.Equal(2, node.Depth);

        node = (StyleRule)node.Children[0]; // h1, ul

        Assert.Equal(3, node.Depth);

        Assert.Equal("h1", node.Selector[0].ToString());
        Assert.Equal("ul", node.Selector[1].ToString());

        var selector = CssWriter.ExpandSelector(node);

        Assert.Equal("#header .inner h1, #header .inner ul", selector.ToString());

        Assert.Equal(
            """
            #header .inner h1,
            #header .inner ul {
              display: table-cell;
              vertical-align: middle;
            }
            """, stylesheet.ToString(), ignoreLineEndingDifferences: true);
    }
}