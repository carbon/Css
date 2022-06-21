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

        Assert.Equal(23, output.Length);
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

              .container {
                min-width: 992px !important;
              }

              .badge {
                border: 1px solid #000;
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

        Assert.Equal(16, mediaRule.Children.Count);

    }

    [Fact]
    public void ChainedColonSelector()
    {
        var text = """
            .card-group > .card:not(:first-child):not(:last-child):not(:only-child) .card-img-top,
            .card-group > .card:not(:first-child):not(:last-child):not(:only-child) .card-img-bottom { color: red; }
            """;

        var sheet = StyleSheet.Parse(text);

        Assert.Equal(".card-group > .card:not(:first-child):not(:last-child):not(:only-child) .card-img-top, .card-group > .card:not(:first-child):not(:last-child):not(:only-child) .card-img-bottom", (sheet.Children[0] as StyleRule).Selector.ToString());
    }

    [Fact]
    public void A()
    {
        var text = """
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
            """;

        var sheet = StyleSheet.Parse(text);

        var rule = sheet.Children[0] as MediaRule;

        Assert.Equal(14, rule.Children.Count);
    }

    [Fact]
    public void QuotedSvgUrl()
    {
        var text = """
.custom-checkbox .custom-control-input:checked ~ .custom-control-label::after {
  background-image: url("data:image/svg+xml;charset=utf8,%3Csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 8 8'%3E%3Cpath fill='%23fff' d='M6.564.75l-3.59 3.612-1.538-1.55L0 4.26 2.974 7.25 8 2.193z'/%3E%3C/svg%3E");
}
""";

        var sheet = StyleSheet.Parse(text);

        var rule = sheet.Children[0] as StyleRule;

        Assert.Equal(".custom-checkbox .custom-control-input:checked ~ .custom-control-label::after", rule.Selector.ToString());
    }

    [Fact]
    public void ParseBootstrap()
    {
        // https://github.com/carbon/Css/issues/1
        var text = File.ReadAllText(TestHelper.GetTestFile("bootstrap.css").FullName);


        var sheet = StyleSheet.Parse(text);

        Assert.Equal(1088, sheet.Children.Count);

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
        var text = """
            @media only screen and (min-width : 1600px) {
              .projects { column-count: 3; }
              .main {   margin-left: 240px; }
              .projectDetails-bottom { display: none; }
              .contactForm { width: 760px; margin: 0 auto; }
            }
            """;

        var css = StyleSheet.Parse(text);

        Assert.Equal(
            """
            @media only screen and (min-width : 1600px) {
              .projects { column-count: 3; }
              .main { margin-left: 240px; }
              .projectDetails-bottom { display: none; }
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
            expected: @"div { transition: width: 5px; }", 
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

        Assert.Equal("hi { hello: test; }", sheet.ToString());
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
        var styles = "main { margin: 0.5in; width: calc(100%/3 - 2*1em - 2*1px); }";

        Assert.Equal("""
            main {
              margin: 0.5in;
              width: calc(100% / 3 - 2 * 1em - 2 * 1px);
            }
            """, StyleSheet.Parse(styles).ToString());
    }

    [Fact]
    public void GitIssue2()
    {
        var styles = 
            """
            .someClass { font-size: 1.2em; }
            .someClass { font-size: 1.2rem; }
            """;

        Assert.Equal("""
            .someClass { font-size: 1.2em; }
            .someClass { font-size: 1.2rem; }
            """, StyleSheet.Parse(styles).ToString());
    }

    [Fact]
    public void CharsetRuleTest()
    {
        // var styles = "@charset \"UTF-8\";", StyleSheet.Parse(styles).ToString());
    }

    [Fact]
    public void ColorTests()
    {
        Assert.Equal("red", CssValue.Parse("red").ToString());

        var value = StyleSheet.Parse("body { background-color: rgba(#ffffff, 0.5) }");

        Assert.Equal("body { background-color: rgba(255, 255, 255, 0.5); }", value.ToString());

    }

    [Fact]
    public void ParseEmpty()
    {
        StyleSheet.Parse("""
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
        var styles = """
            @font-face {
              font-family: 'CMBilling';
              src: url('../fonts/cm-billing-webfont.eot');
              src: url('../fonts/cm-billing-webfont.eot?#iefix') format('embedded-opentype'),
                   url('../fonts/cm-billing-webfont.woff') format('woff');
              font-weight: normal;
              font-style: normal;
            }
            """;

        var sheet = StyleSheet.Parse(styles);

        var value = ((CssDeclaration)((CssRule)sheet.Children[0])[2]).Value;

        //  url('../fonts/cm-billing-webfont.eot?#iefix') format('embedded-opentype'),  url('../fonts/cm-billing-webfont.woff') format('woff')

        var list = (CssValueList)value;
        var list_1_0 = (CssValueList)list[1];

        Assert.Equal(2, list.Count);
        Assert.Equal("url('../fonts/cm-billing-webfont.eot?#iefix') format('embedded-opentype')", list[0].ToString());

        Assert.Equal("url('../fonts/cm-billing-webfont.woff')", list_1_0[0].ToString());
        Assert.Equal("format('woff')", list_1_0[1].ToString().ToString());

        Assert.Equal("""
            @font-face {
              font-family: 'CMBilling';
              src: url('../fonts/cm-billing-webfont.eot');
              src: url('../fonts/cm-billing-webfont.eot?#iefix') format('embedded-opentype'), url('../fonts/cm-billing-webfont.woff') format('woff');
              font-weight: normal;
              font-style: normal;
            }
            """, sheet.ToString());

        // Test 2
        StyleSheet.Parse("""

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
    font-family: 'Colfax-Light-Italic';
    src: url('../fonts/ColfaxWebLightItalicSub.eot');
    src: url('../fonts/ColfaxWebLightItalicSub.eot?#iefix') format('embedded-opentype'),
         url('../fonts/ColfaxWebLightItalicSub.woff') format('woff');
    font-weight: 200;
    font-style: italic;
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
        Assert.Equal(":focus { outline: 0; }", StyleSheet.Parse(":focus { outline:0; }").ToString());

        Assert.Equal(".projects li:first-child { background-color: orange; }", StyleSheet.Parse(".projects li:first-child { background-color: orange; }").ToString());

        Assert.Equal(".projects li:first-child a { background-color: orange; }", StyleSheet.Parse(".projects li:first-child a { background-color: orange; }").ToString());
    }

    [Fact]
    public void Parse3()
    {
        var styleSheet = """
            div#profile_locationEdit table				{ width:100%; }
            div#profile_locationEdit table td 			{ padding:2px 0; }
            div#profile_locationEdit table td.addLab 	{ width:90px; font-size:13px; }

            table.info { width:100%; margin:16px 0 0 0; padding:0; }
            table.info td { vertical-align:top; }
            table.info td p { margin:0 0 16px 0; line-height:20px; }
            table.info td p.tiny { margin-bottom:0; color:#999; }
    
            div.body p { margin:0 0 16px 0; line-height:20px; }

            .availabilityRow { width:760px; padding:20px 0; }

            .availabilityRow .controlRow { display:block; margin-left:20px; }
    
            .availabilityRow .controlRow a.button {
                display:block;
                float:left;
                width:190px; height:50px;
                margin:0 12px 0 0;
                border:2px solid #000;
                background: #000 url('../img/project/bg_buttonBlack.png') repeat-x 0 0;
                color:#f4f4f4;
                font-size:16px;
                text-align:center;
                line-height:50px;
                border-radius:8px;
                box-shadow:0 0 5px 1px rgba(0, 0, 0, 0.3);
                filter:alpha(opacity=40); opacity:0.4;
            }

            .availabilityRow .controlRow a.button:hover { border:2px solid #6CC3F4; }
            .availabilityRow .controlRow a.button.selected {
                box-shadow:0 0 5px 2px #6CC3F4;
                border:2px solid #000;
                cursor:default;
                filter:alpha(opacity=100); opacity:1.0;
                }
    
            .availabilityRow .controlRow .or {
                float:left;
                height:50px;
                margin:0 12px 0 0;
                color:#999;
                line-height:50px;
                }

            html, body, div, span, applet, object, iframe,
            h1, h2, h3, h4, h5, h6, p, blockquote, pre,
            a, abbr, acronym, address, big, cite, code,
            del, dfn, em, font, img, ins, kbd, q, s, samp,
            small, strike, strong, sub, sup, tt, var,
            b, u, i, center,
            dl, dt, dd, ol, ul, li,
            fieldset, form, label, legend,
            table, caption, tbody, tfoot, thead, tr, th, td {
                margin:0;
                padding:0;
                border:0;
                outline:0;
                font-size:100%;
                vertical-align:baseline;
                background:transparent;
            }

            ol, ul { list-style:none; }

            :focus { outline:0; }

            button:hover, button:active, button:focus {
                cursor: pointer;
                outline: none;
                -moz-outline-style: none;
            }
    

            article,footer,header,section { margin:0; padding:0; display:block; }
            aside,details { margin:0; padding:0; }
    
            /*============================================================
             Defaults
            ============================================================*/
            a { cursor:pointer; outline:none; text-decoration:none; }
            a:active, a:focus { outline: none; }
            a:link { text-decoration:none; }
            a:visited { text-decoration:none; }
            a:hover { text-decoration:none; }
            a img { border:0; }

            input, input::-moz-focus-inner, button, button::-moz-focus-inner { border:0; }

            img { vertical-align: middle; } 

            em { font-style:italic; }
            strong { font-weight:bold; }
            textarea { font-family: Helvetica, Arial, sans-serif; }

            html, body, div, span, applet, object, iframe,
            h1, h2, h3, h4, h5, h6, p, blockquote, pre,
            a, abbr, acronym, address, big, cite, code,
            del, dfn, em, font, img, ins, kbd, q, s, samp,
            small, strike, strong, sub, sup, tt, var,
            b, u, i, center,
            dl, dt, dd, ol, ul, li,
            fieldset, form, label, legend,
            table, caption, tbody, tfoot, thead, tr, th, td {
                margin:0;
                padding:0;
                border:0;
                outline:0;
                font-size:100%;
                vertical-align:baseline;
                background:transparent;
                }
            ol, ul { list-style:none; }

            h1, h2, h3, h4, h5, h6 {
                font-weight: normal;
            }
    
            /* = HTML 5
            ----------------------------------------------- */
            article,footer,header,section { margin:0; padding:0; display:block; }
            aside,details { margin:0; padding:0; }


            /*============================================================
             Clearfix
            ============================================================*/

            .clearfix {
                display: inline-block;
            }

            body.account strong {
                font-family: 'Colfax-Medium';
            }

            body.account .left ul {
                font-size: 1.8em;
                margin-top: 6px;
            }

            body.account .left ul li {
                height: 36px;
                text-align: center;
                margin-bottom: 12px;
            }

            body.account .left ul li span {
                color: #E9F6FF;
                position: relative;
                margin-right: 5px;
                width: 75px;
                text-align: center;
            }

            body.account .right ul li:before {
                content:'\2022';
                font: 1.2em 'CMBilling';
                position: relative;
                top: 0;
                margin-right: 10px;
            }

            body.account .right ul {
                padding-left: 90px;
            }

            body.account .divider {
                position: absolute;
                width: 20px;
                text-align: center;
                left: 50%;
                margin-left: -10px;
                float: left;
                opacity: 1;
                padding-top: 9px;
                line-height: 2em;
            }

            body.account .divider ul li.line {
                opacity: .3;
                width: 1px;
                margin-left: 9px;
                border-left: 1px solid rgba(36, 76, 102, 1);
                margin-bottom: 3px;
            }

            body.account .message h1 {
                text-align: center;
                color: #244D66;
                text-shadow: 0 2px 0px rgba(255, 255, 255, 0.3);
                font: 3em 'Colfax-Medium';
            }

            body.account .subtitle {
                text-align: center;
                margin-bottom: 2em;
                font-size: .8em;
                top: -12px;
                position: relative;
                opacity: .5;
            }

            body.account .content {
                color: #4C4C4C;
                width: 800px;
                margin-left: auto;
                margin-right: auto;
                border-radius: 4px;
                margin-bottom: 50px;
                position: relative;
            }

            body.account div.message {
                padding: 30px;
                border-top-left-radius: 4px;
                border-top-right-radius: 4px;
                -moz-box-shadow: 0 0 8px rgba(0, 0, 0, 0.15);
                -webkit-box-shadow: 0 0 8px rgba(0, 0, 0, 0.15);
                box-shadow: 0 0 8px rgba(0, 0, 0, 0.15);
                position: relative;
                z-index: 1;

            text-shadow: 0 2px 1px rgba(255, 255, 255, 0.3);
            font-size: 1.2em;
            line-height: 1.5em;

    
            /*	background-color: #62698A;*/

                background: rgb(169,222,254); /* Old browsers */
                background: -moz-radial-gradient(center, ellipse cover,  rgba(169,222,254,1) 0%, rgba(127,193,230,1) 100%); /* FF3.6+ */
                background: -webkit-gradient(radial, center center, 0px, center center, 100%, color-stop(0%,rgba(169,222,254,1)), color-stop(100%,rgba(127,193,230,1))); /* Chrome,Safari4+ */
                background: -webkit-radial-gradient(center, ellipse cover,  rgba(169,222,254,1) 0%,rgba(127,193,230,1) 100%); /* Chrome10+,Safari5.1+ */
                background: -o-radial-gradient(center, ellipse cover,  rgba(169,222,254,1) 0%,rgba(127,193,230,1) 100%); /* Opera 12+ */
                background: -ms-radial-gradient(center, ellipse cover,  rgba(169,222,254,1) 0%,rgba(127,193,230,1) 100%); /* IE10+ */
                background: radial-gradient(center, ellipse cover,  rgba(169,222,254,1) 0%,rgba(127,193,230,1) 100%); /* W3C */
                filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#a9defe', endColorstr='#7fc1e6',GradientType=1 ); /* IE6-9 fallback on horizontal gradient */
    
            }


            body.account .cardNumber span.message {
                color: rgba(97, 104, 138, 0.7);
                position: absolute;
                top: 124px;
                font-size: 11px;
                left: 142px;
            }

            body.account .securityCode span.message {
                color: rgba(97, 104, 138, 0.7);
                position: absolute;
                top: 205px;
                font-size: 11px;
                left: 142px;
            }

            body.account .expiration span.message {
                color: rgba(97, 104, 138, 0.7);
                position: absolute;
                top: 287px;
                font-size: 11px;
                left: 142px;
            }


            body.account span.message {
                display: none;
            }

            body.account .invalid span.message {
                display: inherit;
            }

            body.account .left {
                width: 340px;
                float: left;
            }

            body.account .right {
                padding-left: 370px;
            }



            body.account div.wrapper .data, div.wrapper .action, div.wrapper .message {
                -webkit-transition: all 0.5s ease-in-out;
                -moz-transition: all 0.5s ease-in-out;
                -o-transition: all 0.5s ease-in-out;
            }

            body.account div.data.choosePlanGuts div {
                margin-left: -40px;
                margin-right: -40px;
                -webkit-transition: all 0.5s ease-in-out;
                -moz-transition: all 0.5s ease-in-out;
                -o-transition: all 0.5s ease-in-out;
                text-align: center;
                position: absolute;
                font-family: 'Colfax-Bold';
                width: 800px;
            }


            body.account .yearly .year .top {
                font-size: 54px;
                height: 56px;
                top: 0;
                padding-top: 38px;
                color: #E5C4DC;
                text-shadow: 0 1px 2px rgba(0, 0, 0, 0.3);
                letter-spacing: -2px;
                opacity: 1;
            }


            body.account .yearly .top span {
                font-family: 'Colfax-Medium';
                font-size: 25px;
                top: -17px;
                position: relative;
                margin-right: 7px;
                margin-left: 10px;
                letter-spacing: 0px;
                opacity: 1;
            }

            body.account .yearly .bottom {
                background-color: rgba(231, 245, 255, 0.5);
                color: #244D66;
                font-size: 13px;
                height: 25px;
                cursor: pointer;
                bottom: 0;
                padding-top: 10px;
            }

            body.account .monthly .top {
                font-size: 13px;
                height: 25px;
                cursor: pointer;
                top: 0;
                padding-top: 9px;
                color: #244D66;
            }


            body.account .monthly .bottom {
                background-color: rgba(231, 245, 255, 0.5);
                color: white;
                font-size: 54px;
                height: 176px;
                bottom: 0;
                padding-top: 18px;
                color: #E5C4DC;
                text-shadow: 0 1px 2px rgba(0, 0, 0, 0.3);
                letter-spacing: -2px;
                opacity: 1;
            }

            body.account .monthly .bottom span {
                font-size: 25px;
                font-weight: 500;
                top: -17px;
                position: relative;
                margin-right: 7px;
                margin-left: 10px;
                letter-spacing: 0px;
                opacity: 1;
                font-family: 'Colfax-Medium';
            }

            /*invisible state for transition*/

            body.account .monthly .year div.top {
                font-size: 54px;
                height: 56px;
                top: 0;
                padding-top: 200px;
                color: #E5C4DC;
                text-shadow: 0 1px 2px rgba(0, 0, 0, 0.3);
                letter-spacing: -2px;
                opacity: 0;
            }

            body.account .monthly .year div.top span {
                font-size: 25px;
                font-weight: 500;
                top: -17px;
                position: relative;
                margin-right: 7px;
                margin-left: 10px;
                letter-spacing: 0px;
                padding-top: 200px;
            }

            body.account .yearly .month div.bottom {
                background-color: rgba(231, 245, 255, 0.5);
                color: white;
                font-size: 54px;
                bottom: 0;
                padding-top: 200px;
                color: #E5C4DC;
                text-shadow: 0 1px 2px rgba(0, 0, 0, 0.3);
                letter-spacing: -2px;
                opacity: 1;
            }

            body.account .yearly .month div.bottom span {
                font-size: 25px;
                font-weight: 500;
                top: -17px;
                position: relative;
                margin-right: 7px;
                margin-left: 10px;
                letter-spacing: 0px;
                opacity: 1;
                padding-top: 200px;
            }




            div.wrapper.choosePlan.yearly .data.choosePlanGuts.month { position: absolute; opacity: 0; }
            div.wrapper.choosePlan.yearly .data.choosePlanGuts.year { position: absolute; opacity: 1; z-index: 110; }


            div.wrapper.choosePlan.monthly .data.choosePlanGuts.month { position: absolute; opacity: 1; z-index: 110; }
            div.wrapper.choosePlan.monthly .data.choosePlanGuts.year { position: absolute; opacity: 0; }



            div.wrapper.choosePlan .data.choosePlanGuts { position: absolute; top: 340px; left: 0px; width: 720px; height: 145px; opacity: 1; z-index: 100; }
            div.wrapper.choosePlan .data.enterPaymentDetailsGuts { position: absolute; top: 260px; left: 0px; width: 720px; height: 145px; opacity: 0; }
            div.wrapper.choosePlan .data.upgradedGuts { position: absolute; top: 340px; left: 0px; width: 720px; height: 145px; opacity: 0; }

            div.wrapper.enterPaymentDetails .data.enterPaymentDetailsGuts { position: absolute; top: 340px; left: 0px; width: 720px; height: 380px; opacity: 1; z-index: 100; }
            div.wrapper.enterPaymentDetails .data.choosePlanGuts { position: absolute; top: 340px; left: 0px; width: 720px; height: 380px; opacity: 0; }
            div.wrapper.enterPaymentDetails .data.upgradedGuts { position: absolute; top: 340px; left: 0px; width: 720px; height: 380px; opacity: 0; }

            div.wrapper.upgraded .data.enterPaymentDetailsGuts { position: absolute; top: 340px; left: 0px; width: 720px; height: 45px; opacity: 0; }
            div.wrapper.upgraded .data.choosePlanGuts {	position: absolute; top: 340px; left: 0px; width: 720px; height: 45px; opacity: 0; }
            div.wrapper.upgraded .data.upgradedGuts { position: absolute; top: 340px; left: 0px; width: 720px; height: 45px; opacity: 1; z-index: 100; }


            div.wrapper.choosePlan .message.choosePlanGuts { opacity: 1; position: absolute; width: 740px; height: 280px; z-index: 200; }
            div.wrapper.choosePlan .message.enterPaymentDetailsGuts { opacity: 0; position: absolute; width: 740px; height: 280px; }
            div.wrapper.choosePlan .message.upgradedGuts { opacity: 0; position: absolute; width: 740px; height: 280px; }

            div.wrapper.enterPaymentDetails .message.choosePlanGuts { opacity: 0; position: absolute; width: 740px; height: 280px; }
            div.wrapper.enterPaymentDetails .message.enterPaymentDetailsGuts { opacity: 1; position: absolute; width: 740px; height: 280px; z-index: 200; }
            div.wrapper.enterPaymentDetails .message.upgradedGuts { opacity: 0; position: absolute; width: 740px; height: 280px; }

            div.wrapper.upgraded .message.choosePlanGuts { opacity: 0; position: absolute; width: 740px; height: 280px; }
            div.wrapper.upgraded .message.enterPaymentDetailsGuts { opacity: 0; position: absolute; width: 740px; height: 280px }
            div.wrapper.upgraded .message.upgradedGuts { opacity: 1; position: absolute; width: 740px; height: 280px; z-index: 200; }


            div.wrapper.choosePlan .action.choosePlanGuts { opacity: 1; position: absolute; width: 800px; top: 565px; z-index: 200; }
            div.wrapper.choosePlan .action.enterPaymentDetailsGuts { opacity: 0; position: absolute; width: 760px; top: 465px; }
            div.wrapper.choosePlan .action.upgradedGuts { opacity: 0; position: absolute; width: 760px; top: 565px; }


            div.wrapper.enterPaymentDetails .action.choosePlanGuts { opacity: 0; position: absolute; width: 760px; top: 800px; }
            div.wrapper.enterPaymentDetails .action.enterPaymentDetailsGuts { opacity: 1; position: absolute; width: 800px; top: 800px; z-index: 200; }
            div.wrapper.enterPaymentDetails .action.upgradedGuts { opacity: 0; position: absolute; width: 760px; top: 800px; }

            div.wrapper.upgraded .action.choosePlanGuts { opacity: 0; position: absolute; width: 760px; top: 445px; }
            div.wrapper.upgraded .action.enterPaymentDetailsGuts { opacity: 0; position: absolute; width: 760px; top: 445px; }
            div.wrapper.upgraded .action.upgradedGuts { opacity: 1; position: absolute; width: 800px; top: 445px; z-index: 200; }


            div.wrapper.enterPaymentDetails .data.enterPaymentDetailsGuts.payPal { position: absolute; top: 340px; left: 0px; width: 680px; height: 100px; opacity: 1; z-index: 100; }
            div.wrapper.enterPaymentDetails .action.enterPaymentDetailsGuts.payPal { opacity: 1; position: absolute; width: 760px; top: 520px; z-index: 200; }

            div.data.upgradedGuts p {
                text-align: center;
            }

            body.account .data {
                background-color: white;
                -moz-box-shadow: 0 0 8px rgba(0, 0, 0, 0.15);
                -webkit-box-shadow: 0 0 8px rgba(0, 0, 0, 0.15);
                box-shadow: 0 0 8px rgba(0, 0, 0, 0.15);
                position: relative;
                padding: 40px;
            }

            body.account .choosePlan.yearly .data {
                background: url(../img/yearBG.svg) no-repeat 226px 25px #CAEBFE;
            }

            body.account .choosePlan.monthly .data {
                background: url(../img/yearBG.svg) no-repeat 226px -200px #CAEBFE;
            }

            body.account .choosePlan.monthly .bottom {
                background: url(../img/yearBG.svg) no-repeat 226px 000px rgba(232, 245, 255, 0.50);
            }

            body.account .choosePlan.yearly .bottom {
                background: url(../img/yearBG.svg) no-repeat 226px 200px rgba(232, 245, 255, 0.50);
            }


            body.account .creditCardImg {
                width: 190px;
                height: 35px;
                background: url(../img/sprite_creditCards.png) no-repeat 0 0;
                position: relative;
                display: inline;
                float: right;
                top: -36px;
            }

            body.account form#payment.americanExpress .creditCardImg {
                background: url(../img/sprite_creditCards.png) no-repeat 0 -42px;
            }


            body.account form#payment.visa .creditCardImg {
                background: url(../img/sprite_creditCards.png) no-repeat 0 -84px;
            }

            body.account form#payment.discover .creditCardImg {
                background: url(../img/sprite_creditCards.png) no-repeat 0 -126px;
            }

            body.account form#payment.masterCard .creditCardImg {
                background: url(../img/sprite_creditCards.png) no-repeat 0 -168px;
            }

            body.account .securityCodeImg {
                width: 190px;
                height: 35px;
                background: url(../img/sprite_creditCards.png) no-repeat 0 -251px;
                display: inline;
                float: left;
                position: relative;
                top: -37px;
                left: 297px;
            }

            body.account form#payment.americanExpress .securityCodeImg {
                background: url(../img/sprite_creditCards.png) no-repeat 0 -209px;
            }

            body.account li.cardNumber input { width: 269px; 
            -webkit-appearance: none;}
            body.account li.securityCode input { width: 76px; 
            -webkit-appearance: none;}



            body.account p.fineprint {
                text-align: center;
                color: rgba(97, 104, 138, 0.69);
                line-height: 1.4em;
                font-size: 0.95em;
                position: absolute;
                bottom: 34px;
                width: 680px;
                -webkit-transition: all 0.5s ease-in-out;
                -moz-transition: all 0.5s ease-in-out;
                -o-transition: all 0.5s ease-in-out;
            }

            .monthly p.fineprint.month { opacity: 1; }
            .monthly p.fineprint.year { opacity: 0; }
            .monthly p.fineprint.paypal { opacity: 0; }

            .yearly p.fineprint.month { opacity: 0; }
            .yearly p.fineprint.year { opacity: 1; }
            .yearly p.fineprint.paypal { opacity: 0; }

            .yearly .payPal p.fineprint.paypal { opacity: 1; }
            .yearly .payPal p.fineprint.year { opacity: 0; }
            .monthly .payPal p.fineprint.month { opacity: 0; }
            .monthly .payPal p.fineprint.paypal { opacity: 1; }


            span.withPayPal {
                color: white;
                position: absolute;
                left: 0;
                right: 0;
                opacity: 0;
                -moz-transition: all .5s linear;
                -ms-transition: all .5s linear;
                -webkit-transition: all .5s linear;
                text-shadow: 0 1px 2px rgba(0, 0, 0, 0.3);
            }

            .payPal button.big {
                color: transparent;
                text-shadow: none;
            }

            .payPal span.withPayPal {
                opacity: 1;
                -moz-transition: all .5s linear;
                -ms-transition: all .5s linear;
                -webkit-transition: all .5s linear;
            }


            .expirationMonth label, .expirationYear label {
                background: url(../img/select_arrow.png) no-repeat 142px 14px #DEFAE5;
            }

            body.account #payment fieldset {
                position: relative;
                color: #62698A;
            }

            body.account #payment legend {
                color: #62698A;
                font: 1.2em 'Colfax';
            }

            body.account #payment fieldset ol {
                margin-top: 43px;
                display: inline-block;
                width: 100%;
                border-bottom: 1px solid #EFF0F3;
                padding-bottom: 16px;
                margin-bottom: 36px;
                -moz-transition: all .5s linear;
                -ms-transition: all .5s linear;
                -webkit-transition: all .5s linear;
            }

            body.account .payPal fieldset ol {
                opacity: 0;
            }

            body.account #payment fieldset ol li {
                padding-top: 20px;
                border-top: 1px solid #EFF0F3;
                padding-bottom: 20px;
            }

            body.account #payment fieldset ol label {
                width: 141px;
                display: block;
                float: left;
                position: relative;
                top: 10px;
                font-size: 1 em;
            }

            body.account li.expiration .inputContainer {
                display: inline;
                position: relative;
            }

            body.account li.expiration .inputContainer select {
                position: relative;
                top: 7px;
            }


            body.account .paymentOptions {
                color: rgba(97, 104, 138, 0.5);
                position: absolute;
                right: 0;
                top: 0;
                text-transform: uppercase;
                font: .9em 'Colfax';
            }

            body.account .paymentOptions label {
                cursor: pointer;
            }

            body.account .paymentOptions input  {
                height: 1em;
                width: 1em;
            }

            body.account .paymentOptions span.or {
                color: #bdbec1;
                font-size: .8em;
                margin-right: 10px;
                margin-left: 10px;
                text-transform: lowercase;
            }

            body.account .paymentOptions span  {
                border-radius	: 30px;
                background-color: white;
                padding: 8px 1px 7px 6px;
                margin-right: 6px;
                position: relative;
                top: -1px;
                width: 1em;
                height: 1em;
            }

            body.account .creditCard .paymentOptions #creditCardLabel span  {
                background-color: #EBEBEB;
            }

            body.account .payPal .paymentOptions #paypalLabel span  {
                background-color: #EBEBEB;
            }

            body.account .focused input {
                border: 1px solid #D1D3DC;
                -webkit-box-shadow: 0 0 6px rgba(97, 105, 141, 0.20);
                   -moz-box-shadow: 0 0 6px rgba(97, 105, 141, 0.20);
                        box-shadow: 0 0 6px rgba(97, 105, 141, 0.20);
    
            }

            body.account .invalid input {
                background-color: #4C4C4C;
                color: white;
                background: url(../img/error.png) no-repeat right center #4C4C4C;
    
            }


            body.account select:focus {
                outline: 2px auto -webkit-focus-ring-color;
            }


            body.account .upgradedGuts .data {
                height: 120px;
                padding-top: 80px;
            }

            body.account button.big {
                background-color: #97BC65;
                border-radius: 6px;
                border-style: none;
                width: 300px;
                height: 58px;
                color: #fff;
                cursor: pointer;
                position: relative;
                -moz-transition: all .05s linear;
                -ms-transition: all .05s linear;
                -webkit-transition: all .05s linear;
                border-bottom: 1px solid #2e6e23;
                padding-top: 10px;
                -moz-box-shadow: inset 0 -10px 40px rgba(0, 0, 0, 0.15);
                -webkit-box-shadow: inset 0 -10px 40px rgba(0, 0, 0, 0.15);
                box-shadow: inset 0 -10px 40px rgba(0, 0, 0, 0.15);
                text-shadow: 0 1px 2px rgba(0, 0, 0, 0.6);
            }


            body.account button.big:hover {
                background-color: #97BC65;
                -moz-box-shadow:    inset 0 -10px 40px rgba(0, 0, 0, 0.25);
                -webkit-box-shadow: inset 0 -10px 40px rgba(0, 0, 0, 0.25);
                box-shadow:         inset 0 -10px 40px rgba(0, 0, 0, 0.25); 
    
            }

            body.account button.big:active {
                top: 1px;
                position: relative;
                background-color: #97BC65;
                border-bottom: 1px solid #2e6e23;
                -moz-box-shadow:    inset 0 10px 40px rgba(0, 0, 0, 0.25);
                -webkit-box-shadow: inset 0 10px 40px rgba(0, 0, 0, 0.25);
                box-shadow:         inset 0 10px 40px rgba(0, 0, 0, 0.25); 
    
            }


            /*div.alert.one {
                position: absolute;
                width: 720px;
                z-index: 300;
                text-align: center;
                border-top-left-radius: 4px;
                border-top-right-radius: 4px;
                background-color: #3B4362;
                -moz-box-shadow: 0 8px 10px -5px rgba(0, 0, 0, 0.25);
                -webkit-box-shadow: 0 8px 10px -5px rgba(0, 0, 0, 0.25);
                box-shadow: 0 8px 10px -5px rgba(0, 0, 0, 0.25);
                padding: 20px;
            }*/

            body.account .error {
                position: absolute;
                width: 760px;
                z-index: 300;
                text-align: center;
                border-top-left-radius: 4px;
                border-top-right-radius: 4px;
                background-color: #FB6366;
                -moz-box-shadow: 0 8px 10px -5px rgba(0, 0, 0, 0.25);
                -webkit-box-shadow: 0 8px 10px -5px rgba(0, 0, 0, 0.25);
                box-shadow: 0 8px 10px -5px rgba(0, 0, 0, 0.25);
                padding: 20px;
                opacity: 0;
                -moz-transition: all .3s linear;
                -ms-transition: all .3s linear;
                -webkit-transition: all .3s linear;
            }

            body.account .invalid .error {
                opacity: 1;
            }

            body.account .error h1.message {
                color: white;
                font-size: 1.5em;
                margin-bottom: 10px;
                margin-top: 0;
                font-family: 'Colfax';
                font-weight: normal;
            }

            body.account .error p.description {
                color: #8a2d2d;
                font-size: 1.em;
                line-height: 1.4em;
                font-family: 'Colfax';
            }

            /*	Spinner -------------------------------------------------*/


            div.spinner {
                position: relative;
                width: 54px;
                height: 54px;
                display: inline-block;
            }
    
            div.spinner div {
                width: 6%;
                height: 12%;
                position: absolute;
                opacity: 0;
                -webkit-animation: fade 1s linear infinite;
                -webkit-border-radius: 50px;
                background-color: #B5D2E0;
                right: 11px;
                top: 42px;
            }
    
            div.spinner div.bar1 {-webkit-transform:rotate(0deg) translate(0, -142%); -webkit-animation-delay: 0s;}    
            div.spinner div.bar2 {-webkit-transform:rotate(30deg) translate(0, -142%); -webkit-animation-delay: -0.9167s;}
            div.spinner div.bar3 {-webkit-transform:rotate(60deg) translate(0, -142%); -webkit-animation-delay: -0.833s;}
            div.spinner div.bar4 {-webkit-transform:rotate(90deg) translate(0, -142%); -webkit-animation-delay: -0.75s;}
            div.spinner div.bar5 {-webkit-transform:rotate(120deg) translate(0, -142%); -webkit-animation-delay: -0.667s;}
            div.spinner div.bar6 {-webkit-transform:rotate(150deg) translate(0, -142%); -webkit-animation-delay: -0.5833s;}
            div.spinner div.bar7 {-webkit-transform:rotate(180deg) translate(0, -142%); -webkit-animation-delay: -0.5s;}
            div.spinner div.bar8 {-webkit-transform:rotate(210deg) translate(0, -142%); -webkit-animation-delay: -0.41667s;}
            div.spinner div.bar9 {-webkit-transform:rotate(240deg) translate(0, -142%); -webkit-animation-delay: -0.333s;}
            div.spinner div.bar10 {-webkit-transform:rotate(270deg) translate(0, -142%); -webkit-animation-delay: -0.25s;}
            div.spinner div.bar11 {-webkit-transform:rotate(300deg) translate(0, -142%); -webkit-animation-delay: -0.1667s;}
            div.spinner div.bar12 {-webkit-transform:rotate(330deg) translate(0, -142%); -webkit-animation-delay: -0.0833s;}

            @animation fade {
                from {opacity: 1;}
                to {opacity: 0.25;}
            }

            /* = Layout
            ----------------------------------------------- */
            html { height: 100%; }

            body {
                height: 100%;
                min-width: 1000px; 
                background: #efefef;
                font-family: Helvetica, Arial, sans-serif; 
                color: #666;
                font-size: 13px;
                background: #eee;
                }

            /* = Styles
            ----------------------------------------------- */

            a       { color: #79CFE2; outline: none; text-decoration: none; }
            a:hover { color: #6CC3F4; }

            h1 { font-size: 2.1em; font-weight: normal; letter-spacing: -0.02em; }
            h2 { font-size: 1.9em; font-weight: normal; letter-spacing: -0.03em; }
            h3 { font-size: 1.3em; font-weight: normal;}
            h4 { font-size: 1.1em; }

            p { line-height: 1.2em; }

            textarea 	{ border: 1px solid #666; padding: 2px; font-size: 16px; }
            input 		{ font-family: Helvetica, Arial, sans-serif; }

            /* = Clear fixes
            ----------------------------------------------- */

            .clear {
                clear:both;
                display:block;
                height:0px;
                margin:0; padding:0;
                overflow:hidden;
                font-size:0px;
                }

            """;

        var parser = new CssParser(styleSheet);


        foreach (var block in parser.ReadRules())
        {
            // Console.WriteLine(block);
        }

    }

}
