namespace Carbon.Css.Tests;

public class CalcTests
{
    [Fact]
    public void A()
    {
        var css = StyleSheet.Parse(
            """
            div { 
              width: calc(100vw - 380px - var(--cover-block-padding) * 2) 
            }
            """);

        Assert.Equal(
            """
            div {
              width: calc(100vw - 380px - var(--cover-block-padding) * 2);
            }
            """, css.ToString());
    }

    [Fact]
    public void B()
    {
        var css = StyleSheet.Parse(
            """
            $containerWidth: 50px;
            div { 
              width: calc(100vw - 380px - $containerWidth * 2) 
            }
            """);

        Assert.Equal(
            """
            div {
              width: calc(100vw - 380px - 50px * 2);
            }
            """, css.ToString());
    }

    [Fact]
    public void C()
    {
        var css = StyleSheet.Parse(
            """
            $varName: --containerWidth;
            div { width: calc(380px - var($varName) * 2); }
            """);

        Assert.Equal(
            """
            div {
              width: calc(380px - var(--containerWidth) * 2);
            }
            """, css.ToString());
    }

    [Fact]
    public void D()
    {
        var css = StyleSheet.Parse(
            """
            $varName: --containerWidth;
            div { width: calc(380px - 50px); }
            """);

        Assert.Equal(
            """
            div {
              width: calc(380px - 50px);
            }
            """, css.ToString());
    }

    [Fact]
    public void E()
    {
        var css = StyleSheet.Parse(
            """
            div { width: 50px + 100px; } 
            """);

        Assert.Equal(
            """
            div {
              width: 150px;
            }
            """, css.ToString());
    }

    [Fact]
    public void F()
    {
        var css = StyleSheet.Parse(
            """
            div { width: 50px * 2; } 
            """);

        Assert.Equal(
            """
            div {
              width: 100px;
            }
            """, css.ToString());
    }

    [Fact]
    public void G()
    {
        var css = StyleSheet.Parse(
            """
            $width: 50px + 50px;
            div { width: calc($width); } 
            """);

        Assert.Equal(
            """
            div {
              width: calc(100px);
            }
            """, css.ToString());
    }
}