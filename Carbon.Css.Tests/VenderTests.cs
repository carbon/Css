namespace Carbon.Css.Tests
{
	using System.IO;

	using NUnit.Framework;

	[TestFixture]
	public class VendorTests : FixtureBase
	{
		[Test]
		public void Nested()
		{
			var ss = StyleSheet.Parse(File.ReadAllText(GetTestFile("nested.css").FullName));

			ss.Context.AllowNestedRules();

			ss.Context.SetCompatibility(Browser.Chrome1, Browser.Safari1);


			ss.ExecuteRewriters();

			var rule = ss.Children[1] as StyleRule;

			Assert.AreEqual("#networkLinks .block .edit", rule.Selector.ToString());


			Assert.AreEqual(@"#networkLinks .block .edit {
  opacity: 0;
  position: absolute;
  top: 0;
  bottom: 0;
  right: 20px;
  margin: auto 0;
  width: 26px;
  height: 26px;
  text-align: center;
  background: #3ea9f5;
  cursor: pointer;
  border-radius: 100%;
  z-index: 105;
  -webkit-transition: margin 0.1s ease-out, opacity 0.1s ease-out;
  transition: margin 0.1s ease-out, opacity 0.1s ease-out;
}", rule.ToString());


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


			sheet.Context.SetCompatibility(Browser.Chrome1, Browser.Safari1, Browser.Firefox1, Browser.IE9);

			sheet.ExecuteRewriters();

			Assert.AreEqual(@"body {
  -moz-transform: rotate(90);
  -ms-transform: rotate(90);
  -webkit-transform: rotate(90);
  transform: rotate(90);
}", sheet.ToString());

		}
	}


}