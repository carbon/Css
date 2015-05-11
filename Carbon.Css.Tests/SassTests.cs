﻿namespace Carbon.Css
{
	using Carbon.Css.Tests;
	using NUnit.Framework;
	using System;
	using System.IO;
	using System.Linq;

	[TestFixture]
	public class SassTests : FixtureBase
	{
		[Test]
		public void VariableReferencingVariable()
		{
			var sheet = StyleSheet.Parse(@"
$red: #f00;
$borderColor: $red;

div { color: rgba($borderColor, 0.5); }");

			Assert.AreEqual("div { color: rgba(255, 0, 0, 0.5); }", sheet.ToString());
		}

		[Test]
		public void VariableReferencingItselfThrows()
		{
			var sheet = StyleSheet.Parse(@"
$red: #fff;
$red: $red;	

div { color: $red; }");

			Assert.Throws<Exception>(() => {
				sheet.ToString();
			});
		}

		[Test]
		public void Test78()
		{
			var sheet = StyleSheet.Parse(@"
$red: #f00;
$borderColor: $red;

div { color: darken($red, 10%); }
div { color: lighten($red, 0.2); }");


			Assert.AreEqual(@"div { color: #e50000; }
div { color: #ff3333; }", sheet.ToString());
		}


		/*
		[Test]
		public void LightenTests()
		{
			var sheet = StyleSheet.Parse(@"
div { color: darken(#ccc, .5); }
div { color: lighten(#ccc, 0.2); }");


			Assert.AreEqual(@"div { color: #cdcdcd; }
div { color: #cbcbcb; }", sheet.ToString());
		}
		*/

		[Test]
		public void FuncInMixin()
		{
			var sheet = StyleSheet.Parse(@"
$red: #f00;
$borderColor: $red;


@mixin hi() {
  color: darken($red, 10%);
  color: lighten($red, 0.2);
}

div {
  @include hi;
}

");

			Assert.AreEqual(@"div {
  color: #e50000;
  color: #ff3333;
}", sheet.ToString());
		}

		[Test]
		public void FuncNestedMixin()
		{
			var sheet = StyleSheet.Parse(@"
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


");


			Assert.AreEqual(@"div {
  color: #e50000;
  color: #ff3333;
}
div div {
  color: #b52f38;
  color: #cccccc;
}", sheet.ToString());
		}

		[Test]
		public void Test99()
		{
			var ss = StyleSheet.Parse(@"div {
  .text,
  .placeholderText {
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
}");

			
			Assert.AreEqual(
@"div .text,
div .placeholderText {
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
}", ss.ToString());
		}

		[Test]
		public void Test53()
		{
			var ss = StyleSheet.FromFile(GetTestFile("test53.css"));

			Assert.AreEqual(
@".block ::-webkit-input-placeholder {
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
  0% { box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 1), 0 0 0 3px rgba(248, 202, 92, 0.6); }
  50% { box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 0.4), 0 0 0 3px rgba(248, 202, 92, 0.2); }
  100% { box-shadow: inset 0 0 0 3px rgba(248, 202, 92, 1), 0 0 0 3px rgba(248, 202, 92, 0.2); }
}
@keyframes domainProcessing2 {
  0% { border-color: rgba(248, 202, 92, 0.4); }
  20% { border-color: rgba(248, 202, 92, 0.2); }
  100% { border-color: rgba(248, 202, 92, 0.2); }
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
.block .description { padding: 5px 250px 20px 225px; }", ss.ToString());


		}

		[Test]
		public void Test1()
		{
			var sheet = StyleSheet.Parse(
				@"nav {
  display: block;
  ul {
    margin: 0;
    padding: 0;
    list-style: none;
  }

  li { display: inline-block; }

  a {
    display: block;
    padding: 6px 12px;
    text-decoration: none;
  }
}");



			var rule = (CssRule)sheet.Children[0];

			var rewriter = new CssWriter(new StringWriter(), sheet.Context);

			var rules = rewriter.Rewrite(rule).ToArray();

			Assert.AreEqual(4, rules.Length);
			Assert.AreEqual("nav { display: block; }", rules[0].ToString());
			Assert.AreEqual(@"nav ul {
  margin: 0;
  padding: 0;
  list-style: none;
}", rules[1].ToString());




			// Ensure the original rule was not modified
			
			/*
			Assert.AreEqual(
@"nav {
  display: block;
  ul {
    margin: 0;
    padding: 0;
    list-style: none;
  }
  li { display: inline-block; }
  a {
    display: block;
    padding: 6px 12px;
    text-decoration: none;
  }
}", sheet.ToString());
			*/
		}
	}
}