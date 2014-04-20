namespace Carbon.Css
{
	using NUnit.Framework;
	using System;

	[TestFixture]
	public class ScssTests
	{
		/*
		[Test]
		public void ParseNested()
		{
			string text = @"#main {
				  color: blue;
				  font-size: 0.3em;

				  a {
					font: {
					  weight: bold;
					  family: serif;
					}
					&:hover {
					  background-color: #eee;
					}
				  }
				}";

			var nested = StyleSheet.Parse(text);
		}
		*/

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

			var mixin = StyleSheet.Parse(text);

			Assert.AreEqual(1, mixin.Context.Mixins.Count);

			throw new Exception(mixin.ToString());

			Assert.AreEqual("", mixin.ToString());
		

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

			var include = ss.Rules[0].Children[0] as IncludeNode;
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
