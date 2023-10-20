namespace Carbon.Css.Tests;

public class FunctionTests
{
    [Fact]
    public void LightenAndDarken()
    {
        var sheet = StyleSheet.Parse(
            """
            $red: #f00;
            $borderColor: $red;

            div { color: darken($red, 10%); }
            div { color: lighten($red, 0.2); }
            """);


        Assert.Equal(
            """
            div {
              color: #c00;
            }
            div {
              color: #f66;
            }
            """, sheet.ToString());
    }

    [Fact]
    public void FuncInMixin()
    {
        var sheet = StyleSheet.Parse(
            """
            $red: #f00;
            $borderColor: $red;

            @mixin hi() {
              color: darken($red, 10%);
              color: lighten($red, 0.2);
            }

            div {
              @include hi;
            }
            """);

        Assert.Equal("""
            div {
              color: #c00;
              color: #f66;
            }
            """, sheet.ToString());
    }

    [Fact]
    public void FuncNestedMixin()
    {
        var sheet = StyleSheet.Parse(
            """
            $red: #f00;
            $borderColor: $red;

            @mixin hi($a, $b) {
              color: darken($red, 10%);
              color: lighten($red, 0.2);
              
              div { 
                color: darken(#e33b47, 20%);
                color: darken($a, 20%);
              }
            }

            div {
              @include hi(#fff);
            }
            """);

        Assert.Equal(
            """
            div {
              color: #c00;
              color: #f66;
            }
            div div {
              color: #a11721;
              color: #ccc;
            }
            """, sheet.ToString());
    }

    [Fact]
    public void Test99()
    {
        var ss = StyleSheet.Parse(
            """
            div {
              .text,
              .placeholderText {
                display: block;
                padding: 0 50px 0 225px;
                line-height: 60px;
                font-size: 18px;
                text-overflow: ellipsis;
                 white-space: nowrap;
                -webkit-font-smoothing: antialiased;
                z-index: 2;
              }
            }
            """);

        Assert.Equal(
            """
            div .text,
            div .placeholderText {
              display: block;
              padding: 0 50px 0 225px;
              line-height: 60px;
              font-size: 18px;
              text-overflow: ellipsis;
              white-space: nowrap;
              -webkit-font-smoothing: antialiased;
              z-index: 2;
            }
            """, ss.ToString());
    }

    [Fact]
    public void Test53()
    {
        var ss = StyleSheet.FromFile(TestHelper.GetTestFile("test53.css"));

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
        .block.empty ::-webkit-input-placeholder {
          color: #b5ada9;
          font-weight: 400;
        }
        .block.empty :-ms-input-placeholder {
          color: #b5ada9;
          font-weight: 400;
        }
        .block.empty ::-moz-placeholder {
          color: #b5ada9;
          font-weight: 400;
        }
        .block.empty :-moz-placeholder {
          color: #b5ada9;
          font-weight: 400;
        }
        @keyframes domainProcessing {
          0% {
            box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 1), 0 0 0 3px rgba(248, 202, 92, 0.6);
          }
          50% {
            box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 0.4), 0 0 0 3px rgba(248, 202, 92, 0.2);
          }
          100% {
            box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 1), 0 0 0 3px rgba(248, 202, 92, 0.2);
          }
        }
        @keyframes domainProcessing2 {
          0% {
            border-color: rgba(248, 202, 92, 0.4);
          }
          20% {
            border-color: rgba(248, 202, 92, 0.2);
          }
          100% {
            border-color: rgba(248, 202, 92, 0.2);
          }
        }
        .block {
          position: relative;
          min-height: 60px;
          border-bottom: solid 1px #f4f4f4;
          background: #fff;
          z-index: 1;
        }
        .block label {
          position: absolute;
          top: 10px;
          left: 0;
          width: 200px;
          line-height: 60px;
          font-size: 16px;
          font-weight: 500;
          text-align: right;
          color: #4c4c4c;
          user-select: none;
          z-index: 3;
        }
        .block .text,
        .block .placeholderText {
          position: relative;
          display: block;
          overflow: hidden;
          padding: 0 50px 0 225px;
          line-height: 60px;
          font-size: 18px;
          font-weight: 400;
          color: #333;
          text-overflow: ellipsis;
          white-space: nowrap;
          -webkit-font-smoothing: antialiased;
          z-index: 2;
        }
        .block .description p {
          font-size: 15px;
          line-height: 20px;
          color: #a3a3a3;
        }
        .block .description {
          padding: 5px 250px 20px 225px;
        }
        """, ss.ToString());
    }
}