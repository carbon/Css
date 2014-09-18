namespace Carbon.Css
{
	using Carbon.Css.Parser;
	using Carbon.Css.Tests;
	using NUnit.Framework;
	using System;
	using System.IO;

	[TestFixture]
	public class MixinTests : FixtureBase
	{
		[Test]
		public void ParseMixin3()
		{
			var mixins = StyleSheet.Parse(File.ReadAllText(GetTestFile("mixins2.css").FullName));

			Assert.AreEqual(13, mixins.Context.Mixins.Count);


			var ss = StyleSheet.Parse(@".happy {
				@include dl-horizontal;
				font-size: 15px;
			} ", mixins.Context);

			ss.Context.AllowNestedRules();

			ss.ExecuteRewriters();

			Assert.AreEqual(
@".happy { font-size: 15px; }
.happy dt {
  text-align: left;
  overflow: hidden;
  white-space: nowrap;
  text-overflow: ellipsis;
  float: left;
  width:   7.5em;
}
.happy dd { padding-left: (   7.5em +   0.625em; }", ss.ToString());
		}

		[Test]
		public void ParseMixin4()
		{
			var mixins = StyleSheet.Parse(File.ReadAllText(GetTestFile("mixins2.css").FullName));

			Assert.AreEqual(13, mixins.Context.Mixins.Count);


			var ss = StyleSheet.Parse(@".happy {
				@include li-horizontal;
			} ", mixins.Context);


			ss.Context.AllowNestedRules();

			ss.ExecuteRewriters();

			// throw new Exception(ss.ToString()); 

			Assert.AreEqual(@".happy li h5 {
  position: inherit;
  text-align: left;
  display: inline-block;
  margin-right:   0.625em;
  overflow: hidden;
  white-space: nowrap;
  text-overflow: ellipsis;
  width: inherit;
}
.happy li p {
  display: inline;
  padding-left: 0;
}
.happy li { padding-bottom: 1em; }", ss.ToString());
		}

		[Test]
		public void ParseMixin()
		{
			var text = @"@mixin left($dist, $x: 1) {
							margin-left: $dist;
							float: left;
							apples: bananas;
							
						}

						main { 
							@include left(50px);
						}
						";

			var ss = StyleSheet.Parse(text);

			ss.Context.AllowNestedRules();

			ss.ExecuteRewriters();

			Assert.AreEqual(1, ss.Context.Mixins.Count);

			Assert.AreEqual(@"main {
  margin-left: 50px;
  float: left;
  apples: bananas;
}", ss.ToString());


		}


		[Test]
		public void ParseMixin2()
		{
			var text = 
			@"@mixin round($radius) {
				border-radius: $radius;
				-webkit-border-radius: $radius;
			}

			main { 
				@include round(50px, 20px);
			}";

			var ss = StyleSheet.Parse(text);

			var rules = ss.GetRules();

			var include = rules[0].Children[0] as IncludeNode;
			var args = include.Args as CssValueList;

			Assert.AreEqual(2, args.Children.Count);
			Assert.AreEqual("50px, 20px", args.ToString());
			Assert.AreEqual(include.Name, "round");

			Assert.AreEqual("50px", args.Children[0].ToString());
			Assert.AreEqual("20px", args.Children[1].ToString());

			Assert.AreEqual(ValueListSeperator.Comma, args.Seperator);

			Assert.AreEqual(1, ss.Context.Mixins.Count);

			ss.Context.AllowNestedRules();

			ss.ExecuteRewriters();


			Assert.AreEqual(@"main {
  border-radius: 50px;
  -webkit-border-radius: 50px;
}", ss.ToString());


		}
	}
}
