namespace Carbon.Css.Tests
{
	using System;
	using System.Linq;

	using NUnit.Framework;

	using Carbon.Css.Parser;

	[TestFixture]
	public class CssTests
	{
		[Test]
		public void Modes()
		{
			var mode = new LexicalModeContext(LexicalMode.Selector);

			Assert.AreEqual(LexicalMode.Selector, mode.Current);

			mode.Enter(LexicalMode.Block);

			Assert.AreEqual(LexicalMode.Block, mode.Current);

			mode.Enter(LexicalMode.Value);

			Assert.AreEqual(LexicalMode.Value, mode.Current);

			mode.Leave(LexicalMode.Value);

			Assert.AreEqual(LexicalMode.Block, mode.Current);

			mode.Leave(LexicalMode.Block);

			Assert.AreEqual(LexicalMode.Selector, mode.Current);
		}

		[Test]
		public void ParseSelector()
		{
			var sheet = StyleSheet.Parse("div > h1 { width: 100px; }");

			var style = sheet.Rules[0] as CssRule;

			Assert.AreEqual(1, sheet.Rules.Count);
			Assert.AreEqual(RuleType.Style, style.Type);
			Assert.AreEqual("div > h1", style.Selector.ToString());
			Assert.AreEqual(1, style.Declarations.Count);
			Assert.AreEqual("width", style.Declarations[0].Name);
			Assert.AreEqual("100px", style.Declarations[0].Value.ToString());
			Assert.AreEqual("div > h1 { width: 100px; }", sheet.ToString());
		}

		[Test]
		public void ImportSelector()
		{
			var sheet = StyleSheet.Parse("@import url(core.css);");

			var style = sheet.Rules[0] as CssRule;

			Assert.AreEqual(1, sheet.Rules.Count);
			Assert.AreEqual(RuleType.Import, style.Type);
			Assert.AreEqual("@import", style.Selector.ToString());
			// Assert.AreEqual(style", "url(core.css)");
		}

		[Test]
		public void ParseSelector2()
		{
			var sheet = StyleSheet.Parse("#monster { font-color: red; background-color: url(http://google.com); }");

			var style = sheet.Rules[0] as CssRule;

			Assert.AreEqual(1, sheet.Rules.Count);
			Assert.AreEqual(RuleType.Style, style.Type);
			Assert.AreEqual("#monster", style.Selector.ToString());
			Assert.AreEqual(2, style.Declarations.Count);
			Assert.AreEqual("font-color", style.Declarations[0].Name);
			Assert.AreEqual("red", style.Declarations[0].Value.ToString());
			Assert.AreEqual("background-color", style.Declarations[1].Name);
			Assert.AreEqual("url(http://google.com)", style.Declarations[1].Value.ToString());
		}

		[Test]
		public void ParseAtRule()
		{
			var sheet = StyleSheet.Parse(@"@-webkit-keyframes fade {
	from {opacity: 1;}
	to {opacity: 0.25;}
}");
			Assert.AreEqual(1, sheet.Rules.Count);
			Assert.AreEqual("@-webkit-keyframes fade", sheet.Rules[0].Selector.Text);

			Assert.AreEqual(
@"@-webkit-keyframes fade {
  from { opacity: 1; }
  to { opacity: 0.25; }
}", sheet.ToString());



			/*
			var tokenizer = new CssTokenizer(new SourceReader(sheet.ToString()));

			Token token;

			while ((token = tokenizer.Next()) != null)
			{
				Console.WriteLine(token.Kind + ":" + token.Value);
			}
			*/
		}

		[Test]
		public void Parse2()
		{
			var styles = @"			
body { font-size: 14px; }
body.dark { background-color: #000; }
p { font-color: red; background: url(http://google.com); }
			";

			var parser = new CssParser(styles);


			foreach (var rule in parser.ReadRules())
			{
				foreach (var declaration in rule.Declarations)
				{
					foreach (var value in declaration.Value)
					{
						if (value.Type == CssValueType.Url)
						{
							((CssPrimitiveValue)value).SetText("url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAGQAAABkAQMAAABKLAcXAAAABlBMVEX/AAAAAP9sof2OAAAAIUlEQVR4nGNgGAWjgFTw//8HJF4Dg8CI5aGGxCgYBcQBAMULD/2Zt2wmAAAAAElFTkSuQmCC)");
						}

						// Console.WriteLine(token.Type + ":" + token.ToString());

					}
				}

				Console.WriteLine(rule);
			}

		}

		[Test]
		public void Declares()
		{
			Assert.AreEqual("font-size: 14px", new CssDeclaration("font-size", "14px").ToString());
			Assert.AreEqual("font-size: 14px !important", new CssDeclaration("font-size", "14px", "important").ToString());


			// Console.WriteLine(new CssDeclaration("filter", "alpha(opacity=50)"));
			// Console.WriteLine(new CssDeclaration("-webkit-box-sizing", "border-box"));
		}

		[Test]
		public void PropertiesA()
		{
			// Console.WriteLine(CssPropertyInfo.BoxSizing.GetPrefixedPropertyNames().Length.ToString());
			Console.WriteLine(CssPropertyInfo.Get("font-size"));
			Console.WriteLine(CssPropertyInfo.Get("box-sizing"));
			Console.WriteLine(CssPropertyInfo.Get("-webkit-box-sizing"));
			Console.WriteLine(CssPropertyInfo.Get("-non-standards"));
		}

		[Test]
		public void Transform()
		{
			var sheet = StyleSheet.Parse(
@"
body { 
  transform: rotate(90);
}
");


			sheet.SetCompatibility(Browser.Chrome1);

			Assert.AreEqual(@"body {
  -moz-transform: rotate(90);
  -ms-transform: rotate(90);
  -o-transform: rotate(90);
  -webkit-transform: rotate(90);
  transform: rotate(90);
}", sheet.ToString());

		}

		[Test]
		public void VariableTests()
		{
			var sheet = StyleSheet.Parse(
@"
:root {
	var-blue: #dceef7;
	var-yellow: #fff5cc;
}

body { 
  background-color: $blue;
  color: $yellow;
}
");
			sheet.EvaluateVariables(removeRootRule: true);

			Assert.AreEqual(
@"body {
  background-color: #dceef7;
  color: #fff5cc;
}", sheet.ToString());
		}

		[Test]
		public void Parse10()
		{
			var styles =
@"
:root { 
	var-addYellow: #fff5cc;
	var-editBlue: #dceef7;
}
body { font-size: 14px; opacity: 0.5; }
.editBlock button.save { background: $addYellow; }
.editBlock.populated button.save { background: $editBlue; }
.rotatedBox { -webkit-box-sizing: border-box; box-sizing: border-box; }
			";

			var sheet = StyleSheet.Parse(styles);

			// Adds a filter: alpha(opacity) property to support opacity in IE8
			// Ads vendor prefixed properties for box-sizing for Safari (Firefox 4 natively implements it)
			sheet.SetCompatibility(Browser.Chrome10, Browser.Firefox4, Browser.IE8, Browser.Safari5);

			// Replaces all the instances of the CSS variables
			sheet.EvaluateVariables();

			Console.WriteLine(sheet.ToString());
		}

		/*
		.box { padding-left: 10px; padding-right: 15px; }
		.box { padding: 1px 2px 3px 4px; padding-right: 5px; }
		*/

		[Test]
		public void Parse3()
		{

			var styleSheet = @"
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

@font-face {
    font-family: 'Colfax-Regular-Italic';
    src: url('../fonts/ColfaxWebRegularItalicSub.eot');
    src: url('../fonts/ColfaxWebRegularItalicSub.eot?#iefix') format('embedded-opentype'),
         url('../fonts/ColfaxWebRegularItalicSub.woff') format('woff');
    font-weight: 300;
    font-style: italic;
}

@font-face {
    font-family: 'Colfax-Medium';
    src: url('../fonts/ColfaxWebMediumSub.eot');
    src: url('../fonts/ColfaxWebMediumSub.eot?#iefix') format('embedded-opentype'),
         url('../fonts/ColfaxWebMediumSub.woff') format('woff');
    font-weight: 400;
    font-style: normal;
}

@font-face {
    font-family: 'Colfax-Medium-Italic';
    src: url('../fonts/ColfaxWebMediumItalicSub.eot');
    src: url('../fonts/ColfaxWebMediumItalicSub.eot?#iefix') format('embedded-opentype'),
         url('../fonts/ColfaxWebMediumItalicSub.woff') format('woff');
    font-weight: 400;
    font-style: italic;
}


@font-face {
    font-family: 'Colfax-Bold';
    src: url('../fonts/ColfaxWebBoldSub.eot');
    src: url('../fonts/ColfaxWebBoldSub.eot?#iefix') format('embedded-opentype'),
         url('../fonts/ColfaxWebBoldSub.woff') format('woff');
    font-weight: 500;
    font-style: normal;
}

@font-face {
    font-family: 'Colfax-Bold-Italic';
    src: url('../fonts/ColfaxWebBoldItalicSub.eot');
    src: url('../fonts/ColfaxWebBoldItalicSub.eot?#iefix') format('embedded-opentype'),
         url('../fonts/ColfaxWebBoldItalicSub.woff') format('woff');
    font-weight: 500;
    font-style: italic;
}

@font-face {
    font-family: 'Colfax-Black';
    src: url('../fonts/ColfaxWebBlackSub.eot');
    src: url('../fonts/ColfaxWebBlackSub.eot?#iefix') format('embedded-opentype'),
         url('../fonts/ColfaxWebBlackSub.woff') format('woff');
    font-weight: 600;
    font-style: normal;
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

@font-face fade {
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
";


			var parser = new CssParser(styleSheet);


			foreach (var block in parser.ReadRules())
			{
				// Console.WriteLine(block);
			}

		}

	}
}


