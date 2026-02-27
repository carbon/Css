namespace Carbon.Css.Tests;

public class NestedMediaRuleTests
{
    [Fact]
    public void NestedMediaRule_BubblesUp()
    {
        var css = StyleSheet.Parse(
            """
            .widget {
              color: red;
              @media (min-width: 600px) {
                color: blue;
              }
            }
            """);

        Assert.Equal(
            """
            .widget {
              color: red;
            }
            @media (min-width: 600px) {
              .widget {
                color: blue;
              }
            }
            """, css.ToString(), ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void NestedMediaRule_WithMultipleDeclarations()
    {
        var css = StyleSheet.Parse(
            """
            .card {
              padding: 1rem;
              font-size: 14px;
              @media (min-width: 768px) {
                padding: 2rem;
                font-size: 16px;
              }
            }
            """);

        Assert.Equal(
            """
            .card {
              padding: 1rem;
              font-size: 14px;
            }
            @media (min-width: 768px) {
              .card {
                padding: 2rem;
                font-size: 16px;
              }
            }
            """, css.ToString(), ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void NestedMediaRule_WithNestedSelector()
    {
        var css = StyleSheet.Parse(
            """
            .container {
              width: 100%;
              .item {
                flex: 1;
                @media (min-width: 1024px) {
                  flex: none;
                  width: 50%;
                }
              }
            }
            """);

        Assert.Equal(
            """
            .container {
              width: 100%;
            }
            .container .item {
              flex: 1;
            }
            @media (min-width: 1024px) {
              .container .item {
                flex: none;
                width: 50%;
              }
            }
            """, css.ToString(), ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void NestedMediaRule_WithNestedSelectorBrowserSupportsNesting()
    {
        var css = StyleSheet.Parse(
            """
            //= nesting native

            .container {
              width: 100%;
              .item {
                flex: 1;
                @media (min-width: 1024px) {
                  flex: none;
                  width: 50%;
                }
              }
            }
            """);

        Assert.Equal(
            """
            .container {
              width: 100%;
              .item {
                flex: 1;
                @media (min-width: 1024px) {
                  flex: none;
                  width: 50%;
                }
              }
            }
            """, css.ToString(), ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void NestedMediaRule_WithReference()
    {
        var css = StyleSheet.Parse(
            """
            .btn {
              background: gray;
              &:hover {
                background: blue;
                @media (prefers-reduced-motion: no-preference) {
                  transition: background 0.2s;
                }
              }
            }
            """);

        Assert.Equal(
            """
            .btn {
              background: gray;
            }
            .btn:hover {
              background: blue;
            }
            @media (prefers-reduced-motion: no-preference) {
              .btn:hover {
                transition: background 0.2s;
              }
            }
            """, css.ToString(), ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void NestedMediaRule_MultipleMediaBlocks()
    {
        var css = StyleSheet.Parse(
            """
            .layout {
              display: block;
              @media (min-width: 600px) {
                display: flex;
              }
              @media (min-width: 1200px) {
                display: grid;
              }
            }
            """);

        Assert.Equal(
            """
            .layout {
              display: block;
            }
            @media (min-width: 600px) {
              .layout {
                display: flex;
              }
            }
            @media (min-width: 1200px) {
              .layout {
                display: grid;
              }
            }
            """, css.ToString(), ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void NestedMediaRule_WithMultiSelector()
    {
        var css = StyleSheet.Parse(
            """
            h1, h2, h3 {
              font-weight: bold;
              @media print {
                color: black;
              }
            }
            """);

        Assert.Equal(
            """
            h1, h2, h3 {
              font-weight: bold;
            }
            @media print {
              h1, h2, h3 {
                color: black;
              }
            }
            """, css.ToString(), ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void NestedMediaRule_PreservesOuterDeclarations()
    {
        var css = StyleSheet.Parse(
            """
            .sidebar {
              width: 100%;
              background: white;
              @media (min-width: 768px) {
                width: 300px;
              }
              border: 1px solid gray;
            }
            """);

        var styleRule = css.Children[0] as StyleRule;

        Assert.Equal(4, styleRule.Children.Count);

        Assert.Equal(
            """
            .sidebar {
              width: 100%;
              background: white;
              border: 1px solid gray;
            }
            @media (min-width: 768px) {
              .sidebar {
                width: 300px;
              }
            }
            """, css.ToString(), ignoreLineEndingDifferences: true);

    }

    [Fact]
    public void NestedMediaRule_DeeplyNested()
    {
        var css = StyleSheet.Parse(
            """
            .page {
              .content {
                .article {
                  font-size: 14px;
                  @media (min-width: 768px) {
                    font-size: 16px;
                  }
                }
              }
            }
            """);

        Assert.Equal(
            """
            .page .content .article {
              font-size: 14px;
            }
            @media (min-width: 768px) {
              .page .content .article {
                font-size: 16px;
              }
            }
            """, css.ToString(), ignoreLineEndingDifferences: true);
    }
}