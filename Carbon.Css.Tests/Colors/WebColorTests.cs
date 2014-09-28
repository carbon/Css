namespace Carbon.Css.Tests
{
	using Carbon.Css.Color;
	using NUnit.Framework;

	[TestFixture]
	public class WebColorTests
	{
		[Test]
		public void Webcolor1()
		{
			var color = WebColor.Parse("rgba(197, 20, 37, 0.3)");

			Assert.AreEqual("rgba(197, 20, 37, 0.3)", color.ToRgba());
			Assert.AreEqual("rgba(197, 20, 37, 0.3)", color.ToString());
			Assert.AreEqual("C51425", color.ToHex());


		}

		[Test]
		public void Webcolor2()
		{
			var color = WebColor.Parse("#FF0000");

			Assert.AreEqual(255, color.R);


			var hsl = color.ToHsla();

			Assert.AreEqual("hsla(0,100%,50%,1)", hsl.ToString());
			Assert.AreEqual("hsla(0,100%,100%,1)", hsl.WithL(1).ToString());



			Assert.AreEqual("#FFFFFF", hsl.WithL(1).ToRgb().ToString());



			Assert.AreEqual("#FF0000", hsl.ToRgb().ToString());

		}
	}
}
