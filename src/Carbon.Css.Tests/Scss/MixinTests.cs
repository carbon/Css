namespace Carbon.Css.Tests;

public class MixinTests
{
    [Fact]
    public void MixinTest19()
    {
        var ss = StyleSheet.Parse(
            """
            @mixin serif($fontWeight:300) {
              font-family: 'Merriweather', serif;
              font-weight: $fontWeight;
            }

            h1, h2, h3, h4, h5, h6 {
              @include serif;
              line-height: 1.2em;
              text-rendering: optimizeLegibility;
              margin: 0 0 1rem 0;
            }
            """);

        Assert.Equal("""
            h1, h2, h3, h4, h5, h6 {
              font-family: 'Merriweather', serif;
              font-weight: 300;
              line-height: 1.2em;
              text-rendering: optimizeLegibility;
              margin: 0 0 1rem 0;
            }
            """, ss.ToString());
    }

    [Fact]
    public void MixinTest15()
    {
        var ss = StyleSheet.Parse("""
            $fontWeight: 500;

            @mixin serif($fontWeight: 300) {
              font-family: 'Merriweather', serif;
              font-weight: $fontWeight;
            }

            h1, h2, h3, h4, h5, h6 {
              @include serif;
              line-height: 1.2em;
              text-rendering: optimizeLegibility;
              margin: 0 0 1rem 0;
            }
            """);

        for (var i = 0; i < 100; i++)
        {
            Assert.Equal("""
                h1, h2, h3, h4, h5, h6 {
                  font-family: 'Merriweather', serif;
                  font-weight: 300;
                  line-height: 1.2em;
                  text-rendering: optimizeLegibility;
                  margin: 0 0 1rem 0;
                }
                """, ss.ToString());
        }

    }


    [Fact]
    public void ParseMixin30()
    {
        var ss = StyleSheet.Parse(
            """
            // Mixins
            @mixin dl-horizontal($dlSpacing : 7.5em, $dlGap : 0.625em) {
              dt {
                text-align: left;
                overflow: hidden;
                white-space: nowrap;
                text-overflow: ellipsis;
                float: left;
                width: $dlSpacing;
              }
              dd {
                padding-left: $dlSpacing;
              }
            }

            // CSS
            .left .awards dl,
            .left .exhibitions dl {
              @include dl-horizontal(3.75em, 0.625em);
            }

            """);
        Assert.Single(ss.Context.Mixins);

        Assert.Equal("dl-horizontal", ss.Context.Mixins["dl-horizontal"].Name);
        Assert.Equal(2, ss.Context.Mixins["dl-horizontal"].Parameters.Count);

        Assert.Equal(
            """
            .left .awards dl dt,
            .left .exhibitions dl dt {
              text-align: left;
              overflow: hidden;
              white-space: nowrap;
              text-overflow: ellipsis;
              float: left;
              width: 3.75em;
            }
            .left .awards dl dd,
            .left .exhibitions dl dd { padding-left: 3.75em; }
            """, ss.ToString());
    }

    [Fact]
    public void ParseMixin7()
    {
        var mixins = StyleSheet.Parse(File.ReadAllText(TestHelper.GetTestFile("mixins2.css").FullName));

        Assert.Equal(13, mixins.Context.Mixins.Count);

        var ss = StyleSheet.Parse(
            """
            .happy {
              @include li-horizontal;
              font-size: 15px;
              oranges: a;
            } 
            """, mixins.Context);

        Assert.Equal(
            """
            .happy {
              list-style: none;
              padding: 0;
              font-size: 15px;
              oranges: a;
            }
            .happy li h5 {
              position: inherit;
              text-align: left;
              display: inline-block;
              margin-right: 0.625em;
              overflow: hidden;
              white-space: nowrap;
              text-overflow: ellipsis;
              width: inherit;
            }
            .happy li p {
              display: inline;
              padding-left: 0;
            }
            .happy li { padding-bottom: 1em; }
            """, ss.ToString());
    }

    [Fact]
    public void ParseMixin3()
    {
        var mixins = StyleSheet.Parse(File.ReadAllText(TestHelper.GetTestFile("mixins2.css").FullName));

        Assert.Equal(13, mixins.Context.Mixins.Count);

        var ss = StyleSheet.Parse(
            """
            .happy {
                @include dl-horizontal;
                font-size: 15px;
            } 
            """, mixins.Context);

        Assert.Equal(
            """
            .happy { font-size: 15px; }
            .happy dt {
              text-align: left;
              overflow: hidden;
              white-space: nowrap;
              text-overflow: ellipsis;
              float: left;
              width: 7.5em;
            }
            .happy dd { padding-left: 8.125em; }
            """, ss.ToString());
    }

    [Fact]
    public void ParseMixin4()
    {
        var mixins = StyleSheet.Parse(File.ReadAllText(TestHelper.GetTestFile("mixins2.css").FullName));

        Assert.Equal(13, mixins.Context.Mixins.Count);

        var ss = StyleSheet.Parse("""
            .happy {
              @include li-horizontal;
            } 
            """, mixins.Context);

        Assert.Equal(
            """
            .happy {
              list-style: none;
              padding: 0;
            }
            .happy li h5 {
              position: inherit;
              text-align: left;
              display: inline-block;
              margin-right: 0.625em;
              overflow: hidden;
              white-space: nowrap;
              text-overflow: ellipsis;
              width: inherit;
            }
            .happy li p {
              display: inline;
              padding-left: 0;
            }
            .happy li { padding-bottom: 1em; }
            """, ss.ToString());
    }

    [Fact]
    public void ParseMixin()
    {
        var ss = StyleSheet.Parse(
            """
            @mixin left($dist, $x: 1) {
                margin-left: $dist;
                float: left;
                apples: bananas;                
            }

            main { 
                @include left(50px);
            }                                    
            """);

        Assert.Single(ss.Context.Mixins);

        Assert.Equal(
            """
            main {
              margin-left: 50px;
              float: left;
              apples: bananas;
            }
            """, ss.ToString());
    }

    [Fact]
    public void ParseMixin2()
    {
        var ss = StyleSheet.Parse(
            """
            @mixin round($radius) {
              border-radius: $radius;
              -webkit-border-radius: $radius;
            }

            main { 
              @include round(50px, 20px);
            }
            """);

        var rules = ss.Children.OfType<CssRule>().ToArray();

        var include = rules[0].Children[0] as IncludeNode;
        var args = include.Args as CssValueList;

        Assert.Equal(2, args.Count);
        Assert.Equal("50px, 20px", args.ToString());
        Assert.Equal("round", include.Name);

        Assert.Equal("50px", args[0].ToString());
        Assert.Equal("20px", args[1].ToString());

        Assert.Equal(CssValueSeperator.Comma, args.Seperator);

        Assert.Single(ss.Context.Mixins);

        Assert.Equal(
            """
            main {
              border-radius: 50px;
              -webkit-border-radius: 50px;
            }
            """, ss.ToString());
    }
}