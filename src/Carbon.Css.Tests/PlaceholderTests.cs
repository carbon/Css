namespace Carbon.Css.Tests;

public class PlaceholderTests
{
    [Fact]
    public void InsideMediaBlock()
    {
        var ss = StyleSheet.Parse(
            """
            @media only screen and (max-width: 770px) {
              ::-webkit-input-placeholder { color: rgba(0,0,0,.2); -webkit-font-smoothing: antialiased; }
              :-ms-input-placeholder { color: red; }
            }
            """);

        Assert.Equal(
            """
            @media only screen and (max-width: 770px) {
              ::-webkit-input-placeholder {
                color: rgba(0, 0, 0, 0.2);
                -webkit-font-smoothing: antialiased;
              }
              :-ms-input-placeholder {
                color: red;
              }
            }
            """, ss.ToString());
    }

    [Fact]
    public void PlaceholderTests1()
    {
        var ss = StyleSheet.Parse(
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
            """, ss.ToString());
    }

    [Fact]
    public void PlaceholderTest2()
    {
        var ss = StyleSheet.Parse(
            """
            //= support Safari >= 5
            .block ::placeholder { color: #cfcece ; font-weight: 400; }
            """);

        Assert.Equal(
            """
            .block ::placeholder {
              color: #cfcece;
              font-weight: 400;
            }
            """, ss.ToString());
    }
}