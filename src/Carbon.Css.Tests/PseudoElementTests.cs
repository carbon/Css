namespace Carbon.Css.Tests;

public class PseudoElementTests
{
    [Fact]
    public void A()
    {
        var css = StyleSheet.Parse(
            """
            //= support Safari >= 5
            .block ::-webkit-input-placeholder { color: #cfcece ; font-weight: 400; }
            .block      :-ms-input-placeholder { color: #cfcece ; font-weight: 400; }
            .block          ::-moz-placeholder { color: #cfcece ; font-weight: 400; }
            .block           :-moz-placeholder { color: #cfcece ; font-weight: 400; }
            """);

        Assert.Equal(
            """
            .block ::-webkit-input-placeholder {
              color: #cfcece;
              font-weight: 400;
            }
            .block :-ms-input-placeholder {
              color: #cfcece;
              font-weight: 400;
            }
            .block ::-moz-placeholder {
              color: #cfcece;
              font-weight: 400;
            }
            .block :-moz-placeholder {
              color: #cfcece;
              font-weight: 400;
            }
            """, css.ToString(), ignoreLineEndingDifferences: true);
    }

    [Fact]
    public void B()
    {
        var css = StyleSheet.Parse(
            """        
            .block::after { content: '' }
            """);

        Assert.Equal(
            """
            .block::after {
              content: '';
            }
            """, css.ToString(), ignoreLineEndingDifferences: true);
    }
}