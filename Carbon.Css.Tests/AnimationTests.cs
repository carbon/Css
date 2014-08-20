namespace Carbon.Css
{
	using NUnit.Framework;
	using System;

	[TestFixture]
	public class AnimationTests
	{


		[Test]
		public void AnimationTest()
		{
			var sheet = StyleSheet.Parse("main { animation: rotate 1.5s infinite linear; }");

			sheet.SetCompatibility(Browser.Chrome1, Browser.Safari1);


			sheet.ExecuteRewriters();

			Assert.AreEqual(@"main {
  -webkit-animation: rotate 1.5s infinite linear;
  animation: rotate 1.5s infinite linear;
}", sheet.ToString());

		}

		[Test]
		public void BeforeSyntax()
		{
			// &:before { content: "("; }
		}

		[Test]
		public void KeyframesTest3()
		{
			var sheet = StyleSheet.Parse(@"
//= support Firefox 3+
//= support Safari 5+
@keyframes planet {
  0%   { transform: translate(0, 0px)rotate(0deg); }
  100% { transform: translate(0, 0px)rotate(-360deg); }
}");

			sheet.ExecuteRewriters();

			Assert.AreEqual(@"@-moz-keyframes planet {
  0% { -moz-transform: translate(0, 0px) rotate(0deg); }
  100% { -moz-transform: translate(0, 0px) rotate(-360deg); }
}
@-webkit-keyframes planet {
  0% { -webkit-transform: translate(0, 0px) rotate(0deg); }
  100% { -webkit-transform: translate(0, 0px) rotate(-360deg); }
}
@keyframes planet {
  0% { transform: translate(0, 0px) rotate(0deg); }
  100% { transform: translate(0, 0px) rotate(-360deg); }
}", sheet.ToString());

			/*
			@-webkit-keyframes planet {
  0%   { -webkit-transform: translate(0,0px) rotate(0deg); }
  100% { -webkit-transform: translate(0, 0px) rotate(-360deg); }
}
@keyframes planet {
  0%   { transform: translate(0, 0px)rotate(0deg); }
  100% { transform: translate(0, 0px)rotate(-360deg); }
}
			*/
		}

		[Test]
		public void KeyframesTest2()
		{
			var sheet = StyleSheet.Parse(@"
//= support Safari >= 5
@keyframes flicker {
  0%    { opacity: 1; }
  30%   { opacity: .8; }
  60%   { opacity: 1; }
  100%  { opacity: .6; }
}");

			sheet.ExecuteRewriters();

			Assert.AreEqual(
@"@-webkit-keyframes flicker {
  0% { opacity: 1; }
  30% { opacity: .8; }
  60% { opacity: 1; }
  100% { opacity: .6; }
}
@keyframes flicker {
  0% { opacity: 1; }
  30% { opacity: .8; }
  60% { opacity: 1; }
  100% { opacity: .6; }
}", sheet.ToString());
		}

		[Test]
		public void KeyframesTest()
		{

			/*
			@-webkit-keyframes planet {
  0%   { -webkit-transform: translate(0,0px) rotate(0deg); }
  100% { -webkit-transform: translate(0, 0px) rotate(-360deg); }
}
@keyframes planet {
  0%   { transform: translate(0, 0px)rotate(0deg); }
  100% { transform: translate(0, 0px)rotate(-360deg); }
}
			*/

			var sheet = StyleSheet.Parse(@"@keyframes flicker {
  0%    { opacity: 1; }
  30%   { opacity: .8; }
  60%   { opacity: 1; }
  100%  { opacity: .6; }
}");

			var rule = sheet.Children[0] as KeyframesRule;



			// var rule2 = sheet.Children[0].Clone();

			Assert.AreEqual(RuleType.Keyframes, rule.Type);
			Assert.AreEqual("flicker", rule.Name);

			Assert.AreEqual(4, rule.Children.Count);

			// TODO: KeyframeRule

			Assert.AreEqual("0%", ((StyleRule)rule.Children[0]).Selector.ToString());
			Assert.AreEqual("30%", ((StyleRule)rule.Children[1]).Selector.ToString());
			Assert.AreEqual("60%", ((StyleRule)rule.Children[2]).Selector.ToString());
			Assert.AreEqual("100%", ((StyleRule)rule.Children[3]).Selector.ToString());

			Assert.AreEqual(@"@keyframes flicker {
  0% { opacity: 1; }
  30% { opacity: .8; }
  60% { opacity: 1; }
  100% { opacity: .6; }
}", sheet.ToString());
		}
	}
}
