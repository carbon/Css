namespace Carbon.Css
{
	using NUnit.Framework;
	using System;

	[TestFixture]
	public class MixinTests
	{
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

			var sheet = StyleSheet.Parse(text);

			Assert.AreEqual(1, sheet.Context.Mixins.Count);

			Assert.AreEqual(@"main {
  margin-left: 50px;
  float: left;
  apples: bananas;
}", sheet.ToString());


		}


		[Test]
		public void ParseMixin2()
		{
			var text = @"@mixin round($radius) {
							border-radius: $radius;
							-webkit-border-radius: $radius;
						}

						main { 
							@include round(50px, 20px);
						}
						";

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

			Assert.AreEqual(@"main {
  border-radius: 50px;
  -webkit-border-radius: 50px;
}", ss.ToString());


		}
	}
}
