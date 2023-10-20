namespace Carbon.Css.Tests;

public class ContainerRuleTests
{
    [Fact]
    public void A()
    {
        var sheet = StyleSheet.Parse(
            """
            @container (width > 150px) {
              div { background: red; }
            }
            """);

        Assert.True(sheet.Children[0] is ContainerRule);

        Assert.Equal(
            """
            @container (width > 150px) {
              div {
                background: red;
              }
            }
            """, sheet.ToString());
    }

    [Fact]
    public void CanUseNamedContainerRules()
    {
        var sheet = StyleSheet.Parse(
            """
            @container sidebar (min-width: 700px) {
              .card {
                font-size: max(1.5em, 1.23em + 2cqi);
              }
            }
            """);

        Assert.True(sheet.Children[0] is ContainerRule);


        Assert.Equal(
            """
            @container sidebar (min-width: 700px) {
              .card {
                font-size: max(1.5em, 1.23em + 2cqi);
              }
            }
            """, sheet.ToString());
    }

    [Fact]
    public void Q()
    {
        var sheet = StyleSheet.Parse(
            """
            @container sidebar (min-width: 700px) {
              .card {
                font-size: 5px + 10px;
              }
            }
            """);

        Assert.Equal(
            """
            @container sidebar (min-width: 700px) {
              .card {
                font-size: 15px;
              }
            }
            """, sheet.ToString());
    }
}