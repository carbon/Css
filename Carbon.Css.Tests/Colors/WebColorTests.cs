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
	}
}
