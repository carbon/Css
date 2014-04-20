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
							float: left;
							apples: bananas;
							margin-left: $dist;
						}

						main { 
							@include left(50px);
						}
						";

			var mixin = StyleSheet.Parse(text);

			Assert.AreEqual(1, mixin.Context.Mixins.Count);
			
			Assert.AreEqual("", mixin.ToString());
		}
	}
}
