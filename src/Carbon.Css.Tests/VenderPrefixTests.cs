namespace Carbon.Css.Tests;

public class VendorPrefixTests
{
    [Fact]
    public void DoubleList2()
    {
        var sheet = StyleSheet.Parse("""
            //= support Safari >= 5
            a { transition: transform 0.04s linear, opacity 0.04s linear, visibility 0.04s linear; }
            """);

        Assert.Equal("""
            a {
              -webkit-transition: -webkit-transform 0.04s linear, opacity 0.04s linear, visibility 0.04s linear;
              transition: transform 0.04s linear, opacity 0.04s linear, visibility 0.04s linear;
            }
            """, sheet.ToString());
    }

    [Fact]
    public void DoubleList3()
    {
        var sheet = StyleSheet.Parse(@"
            //= support Safari >= 9
            a { transition: transform 0.04s linear, opacity 0.04s linear, visibility 0.04s linear; }");

        Assert.Equal(@"a { transition: transform 0.04s linear, opacity 0.04s linear, visibility 0.04s linear; }", sheet.ToString());
    }

    [Fact]
    public void UnsupportedBrowser()
    {
        var sheet = StyleSheet.Parse("""
            //= support Safari >= 9
            //= support IE >= 10
            a { transition: transform 0.04s linear, opacity 0.04s linear, visibility 0.04s linear; }
            """);

        Assert.Equal("a { transition: transform 0.04s linear, opacity 0.04s linear, visibility 0.04s linear; }", sheet.ToString());
    }

    [Fact]
    public void ParseWebkitPrefixedKeyframesRule()
    {
        var sheet = StyleSheet.Parse(@"
@-webkit-keyframes fade {
    from {opacity: 1;}
    to {opacity: 0.25;}
}");

        var atRule = (UnknownRule)sheet.Children[0];

        Assert.Equal("-webkit-keyframes", atRule.Name);
        Assert.Equal("fade", atRule.Text.ToString());

        Assert.Equal(
            """
            @-webkit-keyframes fade {
              from { opacity: 1; }
              to { opacity: 0.25; }
            }
            """, sheet.ToString());
    }

    [Fact]
    public void Nested3()
    {
        var ss = StyleSheet.Parse(
            """
            #networkLinks .block .emptyGuts,
            #networkLinks .block .populatedGuts,
            #networkLinks .block .editGuts {
              cursor: default;
              z-index: 100;
            }
            """);

        Assert.Equal(
            """
            #networkLinks .block .emptyGuts,
            #networkLinks .block .populatedGuts,
            #networkLinks .block .editGuts {
              cursor: default;
              z-index: 100;
            }
            """, ss.ToString());
    }

    [Fact]
    public void Nested()
    {
        var ss = StyleSheet.Parse(File.ReadAllText(TestHelper.GetTestFile("nested.css").FullName));

        ss.Context.SetCompatibility(BrowserInfo.Chrome1, BrowserInfo.Safari1);

        Assert.Equal("""
            #networkLinks .block .edit:before {
              font-family: 'carbonmade';
              font-size: 12px;
              line-height: 26px;
              color: #fff;
              text-align: center;
            }
            #networkLinks .block .edit {
              opacity: 0;
              position: absolute;
              top: 0;
              bottom: 0;
              right: 20px;
              margin: auto 0;
              width: 26px;
              height: 26px;
              text-align: center;
              background: #3ea9f5;
              cursor: pointer;
              border-radius: 100%;
              z-index: 105;
              -webkit-transition: margin 0.1s ease-out, opacity 0.1s ease-out;
              transition: margin 0.1s ease-out, opacity 0.1s ease-out;
            }
            #networkLinks .block .destroy:before {
              font-family: 'carbonmade';
              font-size: 17px;
              line-height: 26px;
              color: rgba(0, 0, 0, 0.1);
              text-align: center;
            }
            #networkLinks .block .destroy:hover:before { color: rgba(0, 0, 0, 0.25); }
            #networkLinks .block .destroy {
              display: none;
              position: absolute;
              top: 0;
              bottom: 0;
              right: 60px;
              margin: auto 0;
              width: 26px;
              height: 26px;
              cursor: pointer;
              border-radius: 100%;
              text-align: center;
              z-index: 105;
            }
            #networkLinks .block .input {
              background-color: #fff;
              -webkit-box-shadow: inset 0 0 0 1px #e6e6e6;
              box-shadow: inset 0 0 0 1px #e6e6e6;
              color: #222;
              height: 40px;
              line-height: 24px;
              padding: 5px 6px 5px 165px;
              margin-left: 0;
              -webkit-box-sizing: border-box;
              box-sizing: border-box;
            }
            #networkLinks .block .emptyGuts,
            #networkLinks .block .populatedGuts,
            #networkLinks .block .editGuts {
              cursor: default;
              z-index: 100;
            }
            #networkLinks .block .controls { padding: 5px 0 10px 210px; }
            """, ss.ToString());


    }

    [Fact]
    public void Transform()
    {
        var sheet = StyleSheet.Parse("""
            body { 
              transform: rotate(90);
            }
            """);

        sheet.Context.SetCompatibility(BrowserInfo.Chrome1, BrowserInfo.Safari1, BrowserInfo.Firefox1);

        Assert.Equal("""
            body {
              -moz-transform: rotate(90);
              -webkit-transform: rotate(90);
              transform: rotate(90);
            }
            """, sheet.ToString());
    }


    [Fact]
    public void BackdropFilter()
    {
        var sheet = StyleSheet.Parse("""
            body { 
              backdrop-filter: blur(10px);
            }
            """);

        sheet.Context.SetCompatibility(BrowserInfo.Chrome1, BrowserInfo.Safari10);

        Assert.Equal("""
            body {
              -webkit-backdrop-filter: blur(10px);
              backdrop-filter: blur(10px);
            }
            """, sheet.ToString());
    }

    [Fact]
    public void BackfaceVisibility()
    {
        var sheet = StyleSheet.Parse(
            """
            body { 
              backface-visibility: hidden;
            }
            """);

        sheet.Context.SetCompatibility(BrowserInfo.Safari10);

        Assert.Equal(
            """
            body {
              -webkit-backface-visibility: hidden;
              backface-visibility: hidden;
            }
            """, sheet.ToString());

    }
}
