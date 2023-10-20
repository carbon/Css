using System.Text;

using Carbon.Css.Parser;

namespace Carbon.Css.Tests;

public class CssTests
{
    [Fact]
    public void EnsureFlushBehavior()
    {
        var output = new MemoryStream();

        var sheet = StyleSheet.Parse("body { color: red; }", new CssContext());

        // StreamWriter.Dispose() closes the underlying stream. DO NOT DISPOSE

        var writer = new StreamWriter(output, Encoding.UTF8);

        sheet.WriteTo(writer);

        writer.Flush();

        Assert.Equal(27, output.Length);
    }

    [Fact]
    public void Test1()
    {
        var ss = StyleSheet.Parse(File.ReadAllText(TestHelper.GetTestFile("test1.css").FullName));

        var writer = new StringWriter();

        ss.WriteTo(writer);

        // throw new System.Exception(ss.ToString());

        // throw new System.Exception(ss.ToString());
    }

    [Fact]
    public void ParsePrintMedia()
    {
        var text = """
            @media print {
              *,
              *::before,
              *::after {
                text-shadow: none !important;
                box-shadow: none !important;
              }
              a:not(.btn) {
                text-decoration: underline;
              }
              abbr[title]::after {
                content: " (" attr(title) ")";
              }
              pre {
                white-space: pre-wrap !important;
              }
              pre,
              blockquote {
                border: 1px solid #adb5bd;
                page-break-inside: avoid;
              }

              thead {
                display: table-header-group;
              }

              tr,
              img {
                page-break-inside: avoid;
              }

              p,
              h2,
              h3 {
                orphans: 3;
                widows: 3;
              }

              h2,
              h3 {
                page-break-after: avoid;
              }

              @page {
                size: a3;
              }

              body {
                min-width: 992px !important;
              }

              .table td,
              .table th {
                background-color: #fff !important;
              }
              .table-bordered th,
              .table-bordered td {
                border: 1px solid #dee2e6 !important;
              }
            }
            """;

        var sheet = StyleSheet.Parse(text);

        var mediaRule = sheet.Children[0] as MediaRule;

        Assert.Equal(14, mediaRule.Children.Count);
    }

    [Fact]
    public void ChainedColonSelector()
    {
        var sheet = StyleSheet.Parse(
            """
            .card-group > .card:not(:first-child):not(:last-child):not(:only-child) .card-img-top,
            .card-group > .card:not(:first-child):not(:last-child):not(:only-child) .card-img-bottom { color: red; }
            """);

        Assert.Equal(".card-group > .card:not(:first-child):not(:last-child):not(:only-child) .card-img-top, .card-group > .card:not(:first-child):not(:last-child):not(:only-child) .card-img-bottom", (sheet.Children[0] as StyleRule).Selector.ToString());
    }

    [Fact]
    public void A()
    {
        var sheet = StyleSheet.Parse(
            """
            @media (min-width: 576px) {
              .card-group {
                -ms-flex-flow: row wrap;
                flex-flow: row wrap;
              }
              .card-group > .card {
                -ms-flex: 1 0 0%;
                flex: 1 0 0%;
                margin-bottom: 0;
              }
              .card-group > .card + .card {
                margin-left: 0;
                border-left: 0;
              }
              .card-group > .card:first-child {
                border-top-right-radius: 0;
                border-bottom-right-radius: 0;
              }
              .card-group > .card:first-child .card-img-top,
              .card-group > .card:first-child .card-header {
                border-top-right-radius: 0;
              }
              .card-group > .card:first-child .card-img-bottom,
              .card-group > .card:first-child .card-footer {
                border-bottom-right-radius: 0;
              }
              .card-group > .card:last-child {
                border-top-left-radius: 0;
                border-bottom-left-radius: 0;
              }
              .card-group > .card:last-child .card-img-top,
              .card-group > .card:last-child .card-header {
                border-top-left-radius: 0;
              }
              .card-group > .card:last-child .card-img-bottom,
              .card-group > .card:last-child .card-footer {
                border-bottom-left-radius: 0;
              }
              .card-group > .card:only-child {
                border-radius: 0.25rem;
              }
              .card-group > .card:only-child .card-img-top,
              .card-group > .card:only-child .card-header {
                border-top-left-radius: 0.25rem;
                border-top-right-radius: 0.25rem;
              }
              .card-group > .card:only-child .card-img-bottom,
              .card-group > .card:only-child .card-footer {
                border-bottom-right-radius: 0.25rem;
                border-bottom-left-radius: 0.25rem;
              }
              .card-group > .card:not(:first-child):not(:last-child):not(:only-child) {
                border-radius: 0;
              }
              .card-group > .card:not(:first-child):not(:last-child):not(:only-child) .card-img-top,
              .card-group > .card:not(:first-child):not(:last-child):not(:only-child) .card-img-bottom,
              .card-group > .card:not(:first-child):not(:last-child):not(:only-child) .card-header,
              .card-group > .card:not(:first-child):not(:last-child):not(:only-child) .card-footer {
                border-radius: 0;
              }
            }
            """);

        var rule = sheet.Children[0] as MediaRule;

        Assert.Equal(14, rule.Children.Count);
    }

    [Fact]
    public void QuotedSvgUrl()
    {
        var sheet = StyleSheet.Parse(
            """
            .custom-checkbox .custom-control-input:checked ~ .custom-control-label::after {
              background-image: url("data:image/svg+xml;charset=utf8,%3Csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 8 8'%3E%3Cpath fill='%23fff' d='M6.564.75l-3.59 3.612-1.538-1.55L0 4.26 2.974 7.25 8 2.193z'/%3E%3C/svg%3E");
            }
            """);

        var rule = (StyleRule)sheet.Children[0];

        Assert.Equal(".custom-checkbox .custom-control-input:checked ~ .custom-control-label::after", rule.Selector.ToString());
    }

    [Fact]
    public void ParseBootstrap()
    {
        // https://github.com/carbon/Css/issues/1
        var text = File.ReadAllText(TestHelper.GetTestFile("bootstrap.css").FullName);

        var sheet = StyleSheet.Parse(text);

        Assert.Equal(1083, sheet.Children.Count);

    }

    [Fact]
    public void ParseProject()
    {
        Assert.Throws<UnbalancedBlock>(() =>
        {
            StyleSheet.Parse(File.ReadAllText(TestHelper.GetTestFile("project.css").FullName));
        });
    }

    [Fact]
    public void ParseMixins()
    {
        var ss = StyleSheet.Parse(File.ReadAllText(TestHelper.GetTestFile("mixins.css").FullName));

        Assert.Equal(6, ss.Context.Mixins.Count);

        var fontText = File.ReadAllText(TestHelper.GetTestFile("fonts.css").FullName);

        _ = StyleSheet.Parse(fontText, ss.Context);
    }

    [Fact]
    public void PathTest()
    {
        var key = "/images/hi.gif";

        Assert.Equal("/images/", key.Replace(Path.GetFileName(key), string.Empty));
    }

    [Fact]
    public void MediaQuery()
    {
        var css = StyleSheet.Parse(
            """
            @media only screen and (min-width : 1600px) {
              .projects { column-count: 3; }
              .main {   margin-left: 240px; }
              .projectDetails-bottom { display: none; }
              .contactForm { width: 760px; margin: 0 auto; }
            }
            """);

        Assert.Equal(
            """
            @media only screen and (min-width : 1600px) {
              .projects {
                column-count: 3;
              }
              .main {
                margin-left: 240px;
              }
              .projectDetails-bottom {
                display: none;
              }
              .contactForm {
                width: 760px;
                margin: 0 auto;
              }
            }
            """, css.ToString());
    }

    [Fact]
    public void Modes()
    {
        var mode = new LexicalModeContext(LexicalMode.Selector);

        Assert.Equal(LexicalMode.Selector, mode.Current);

        mode.Enter(LexicalMode.Block);

        Assert.Equal(LexicalMode.Block, mode.Current);

        mode.Enter(LexicalMode.Value);

        Assert.Equal(LexicalMode.Value, mode.Current);

        mode.Leave(LexicalMode.Value);

        Assert.Equal(LexicalMode.Block, mode.Current);

        mode.Leave(LexicalMode.Block);

        Assert.Equal(LexicalMode.Selector, mode.Current);
    }


    [Fact]
    public void ZoomOut()
    {
        var sheet = StyleSheet.Parse(
            """
            //= support Safari >= 7
            div { cursor: zoom-out }
            """
            );

        Assert.Equal(
            """
            div {
              cursor: -webkit-zoom-out;
              cursor: zoom-out;
            }
            """, sheet.ToString());
    }

    [Fact]
    public void GrabSupport()
    {
        var sheet = StyleSheet.Parse(
            """
            //= support Firefox >= 5
            //= support Safari >= 1
            div { cursor: grab }
            """
            );

        Assert.Equal(
            """
            div {
              cursor: -moz-grab;
              cursor: -webkit-grab;
              cursor: grab;
            }
            """, sheet.ToString()
                    );
    }

    [Fact]
    public void DirivitiveComment()
    {
        var sheet = StyleSheet.Parse(
            """
            //= support Firefox >= 5
            //= support Chrome >= 1
            div { transition: width 1s }
            """);

        /*
        Assert.Equal(
            expected: "div { transition: width: 5px; }", 
            actual: sheet.ToString()
        );
        */

        Assert.Equal(
            """
            div {
              -moz-transition: width 1s;
              -webkit-transition: width 1s;
              transition: width 1s;
            }
            """,
            actual: sheet.ToString()
        );
    }

    [Fact]
    public void LineComment()
    {
        var sheet = StyleSheet.Parse(
            """
            // hello
            hi { hello: test; }
            """);

        Assert.Equal(
            """
            hi {
              hello: test;
            }
            """, sheet.ToString());
    }

    [Fact]
    public void ImportSelector()
    {
        var sheet = StyleSheet.Parse("@import url(core.css);");

        var rule = sheet.Children[0] as ImportRule;

        Assert.Single(sheet.Children);
        Assert.Equal(RuleType.Import, rule.Type);
        Assert.Equal("@import url('core.css');", rule.ToString());
        Assert.Equal("url('core.css')", rule.Url.ToString());

        Assert.Equal("@import url('core.css');", sheet.ToString());
    }

    [Fact]
    public void ParseSelector2()
    {
        var sheet = StyleSheet.Parse("#monster { font-color: red; background-color: url(http://google.com); }");

        var style = sheet.Children[0] as StyleRule;

        Assert.Single(sheet.Children);
        Assert.Equal(RuleType.Style, style.Type);
        Assert.Equal("#monster", style.Selector.ToString());
        Assert.Equal(2, style.Children.Count);

        var x = (CssDeclaration)style[0];
        var y = (CssDeclaration)style[1];

        Assert.Equal("font-color", x.Name.ToString());
        Assert.Equal("red", x.Value.ToString());
        Assert.Equal("background-color", y.Name.ToString());
        Assert.Equal("url(http://google.com)", y.Value.ToString());
    }

    [Fact]
    public void Unclosed()
    {
        var sheet = StyleSheet.Parse("div.wrapper.upgraded .message.enterPaymentDetailsGuts { opacity: 0; position: absolute; width: 740px; height: 280px }");

        sheet.ToString();
    }

    [Fact]
    public void CalcTest()
    {
        var sheet = StyleSheet.Parse("main { margin: 0.5in; width: calc(100%/3 - 2*1em - 2*1px); }");

        Assert.Equal(
            """
            main {
              margin: 0.5in;
              width: calc(100% / 3 - 2 * 1em - 2 * 1px);
            }
            """, sheet.ToString());
    }

    [Fact]
    public void GitIssue2()
    {
        var sheet = StyleSheet.Parse(
            """
            .someClass { font-size: 1.2em; }
            .someClass { font-size: 1.2rem; }
            """);

        Assert.Equal(
            """
            .someClass {
              font-size: 1.2em;
            }
            .someClass {
              font-size: 1.2rem;
            }
            """, sheet.ToString());
    }

    [Fact]
    public void CharsetRuleTest()
    {
        // var styles = "@charset \"UTF-8\";", StyleSheet.Parse(styles).ToString());
    }

    [Fact]
    public void ParseEmpty()
    {
        StyleSheet.Parse(
            """
            a {
            }
            """);
    }

    [Fact]
    public void ComponentValues()
    {
        Assert.Equal("red", CssValue.Parse("red").ToString());

        var value = CssValue.Parse("3px 3px rgba(50%, 50%, 50%, 50%), lemonchiffon 0 0 4px inset");
        var value2 = CssValue.Parse(@"""Gill Sans"", Futura, sans-serif");

        Assert.Equal("3px 3px rgba(50%, 50%, 50%, 50%), lemonchiffon 0 0 4px inset", value.ToString());
        Assert.Equal(@"""Gill Sans"", Futura, sans-serif", value2.ToString());
    }

    [Fact]
    public void FontFace()
    {
        var sheet = StyleSheet.Parse(
            """
            @font-face {
              font-family: 'CMBilling';
              src: url('../fonts/cm-billing-webfont.eot');
              src: url('../fonts/cm-billing-webfont.eot?#iefix') format('embedded-opentype'),
                   url('../fonts/cm-billing-webfont.woff') format('woff');
              font-weight: normal;
              font-style: normal;
            }
            """);

        var value = ((CssDeclaration)((CssRule)sheet.Children[0])[2]).Value;

        var list = (CssValueList)value;
        var list_1_0 = (CssValueList)list[1];

        Assert.Equal(2, list.Count);
        Assert.Equal("url('../fonts/cm-billing-webfont.eot?#iefix') format('embedded-opentype')", list[0].ToString());

        Assert.Equal("url('../fonts/cm-billing-webfont.woff')", list_1_0[0].ToString());
        Assert.Equal("format('woff')", list_1_0[1].ToString());

        Assert.Equal(
            """
            @font-face {
              font-family: 'CMBilling';
              src: url('../fonts/cm-billing-webfont.eot');
              src: url('../fonts/cm-billing-webfont.eot?#iefix') format('embedded-opentype'), url('../fonts/cm-billing-webfont.woff') format('woff');
              font-weight: normal;
              font-style: normal;
            }
            """, sheet.ToString());

        // Test 2
        StyleSheet.Parse(
            """
            /*	Font -------------------------------------------------*/

            @font-face {
                font-family: 'CMBilling';
                src: url('../fonts/cm-billing-webfont.eot');
                src: url('../fonts/cm-billing-webfont.eot?#iefix') format('embedded-opentype'),
                     url('../fonts/cm-billing-webfont.woff') format('woff');
                font-weight: normal;
                font-style: normal;
            }

            @font-face {
                font-family: 'Colfax-Thin';
                src: url('../fonts/ColfaxWebThinSub.eot');
                src: url('../fonts/ColfaxWebThinSub.eot?#iefix') format('embedded-opentype'),
                     url('../fonts/ColfaxWebThinSub.woff') format('woff');
                font-weight: 100;
                font-style: normal;
            }

            @font-face {
                font-family: 'Colfax-Thin-Italic';
                src: url('../fonts/ColfaxWebThinItalicSub.eot');
                src: url('../fonts/ColfaxWebThinItalicSub.eot?#iefix') format('embedded-opentype'),
                     url('../fonts/ColfaxWebThinItalicSub.woff') format('woff');
                font-weight: 100;
                font-style: italic;
            }

            @font-face {
                font-family: 'Colfax-Light';
                src: url('../fonts/ColfaxWebLightSub.eot');
                src: url('../fonts/ColfaxWebLightSub.eot?#iefix') format('embedded-opentype'),
                     url('../fonts/ColfaxWebLightSub.woff') format('woff');
                font-weight: 200;
                font-style: normal;
            }

            @font-face {
                font-family: 'Colfax';
                src: url('../fonts/ColfaxWebRegularSub.eot');
                src: url('../fonts/ColfaxWebRegularSub.eot?#iefix') format('embedded-opentype'),
                     url('../fonts/ColfaxWebRegularSub.woff') format('woff');
                font-weight: 300;
                font-style: normal;
            }
            """);
    }

    [Fact]
    public void ParseUnquotedUrl()
    {
        var styles = 
            """
            p { font-color: red; background: url(http://google.com); }
            """;

        var sheet = StyleSheet.Parse(styles);

        var rule = (StyleRule)sheet.Children[0];

        Assert.Equal("http://google.com", ((CssFunction)rule.GetDeclaration("background").Value).Arguments.ToString());
    }

    [Fact]
    public void CssValueTypes()
    {
        Assert.Equal(NodeKind.Number, CssValue.Parse("123").Kind);
        Assert.Equal(NodeKind.Number, CssValue.Parse("-123").Kind);
        Assert.Equal(NodeKind.Number, CssValue.Parse("123.5").Kind);
    }

    [Fact]
    public void Declares()
    {
        Assert.Equal("font-weight: bold", new CssDeclaration("font-weight", "bold").ToString());
        Assert.Equal("font-size: 14px", new CssDeclaration("font-size", "14px").ToString());
        Assert.Equal("font-size: 14px !important", new CssDeclaration("font-size", "14px", "important").ToString());
    }

    [Fact]
    public void ParseDXImageTransformFilter()
    {
        StyleSheet.Parse("body { filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#a9defe', endColorstr='#7fc1e6',GradientType=1 ); } /* IE6-9 fallback on horizontal gradient */");
    }

    [Fact]
    public void Parse11()
    {
        Assert.Equal(
            """
            :focus {
              outline: 0;
            }
            """, StyleSheet.Parse(":focus { outline:0; }").ToString());

        Assert.Equal(
            """
            .projects li:first-child {
              background-color: orange;
            }
            """, StyleSheet.Parse(".projects li:first-child { background-color: orange; }").ToString());

        Assert.Equal(
            """
            .projects li:first-child a {
              background-color: orange;
            }
            """, StyleSheet.Parse(".projects li:first-child a { background-color: orange; }").ToString());
    }

    [Fact]
    public void Parse3()
    {
        var text = File.ReadAllText(TestHelper.GetTestFile("test2.css").FullName);

        var parser = new CssParser(text);

        foreach (var block in parser.ReadRules())
        {
            // Console.WriteLine(block);
        }
    }
}