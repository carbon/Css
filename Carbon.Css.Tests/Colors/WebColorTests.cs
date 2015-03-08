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
			Assert.AreEqual("c51425", color.ToHex());


		}

		[Test]
		public void Lighten()
		{
			/*
			def test_lighten
			assert_equal("#4d4d4d", evaluate("lighten(hsl(0, 0, 0), 30%)"))
			assert_equal("#ee0000", evaluate("lighten(#800, 20%)"))
			assert_equal("white", evaluate("lighten(#fff, 20%)"))
			assert_equal("white", evaluate("lighten(#800, 100%)"))
			assert_equal("#880000", evaluate("lighten(#800, 0%)"))
			assert_equal("rgba(238, 0, 0, 0.5)", evaluate("lighten(rgba(136, 0, 0, 0.5), 20%)"))
			assert_equal("rgba(238, 0, 0, 0.5)", evaluate("lighten($color: rgba(136, 0, 0, 0.5), $amount: 20%)"))
			*/

			Assert.AreEqual("4d4d4d", new Hsla(0, 0, 0).AdjustLightness(.3f).ToRgb().ToHex());
			Assert.AreEqual("ee0000", WebColor.Parse("#800").Lighten(0.2f).ToHex());
			Assert.AreEqual("ffffff", WebColor.Parse("#fff").Lighten(0.2f).ToHex());
			Assert.AreEqual("ffffff", WebColor.Parse("#800").Lighten(1f).ToHex());
			Assert.AreEqual("880000", WebColor.Parse("#800").Lighten(0f).ToHex());
			Assert.AreEqual("rgba(238, 0, 0, 0.5)", WebColor.Parse("rgba(136, 0, 0, 0.5)").Lighten(0.2f).ToString());

			// Assert.AreEqual("hsla(0, 0, 30, 1)", new Hsla(0d, 0d, 0d).AdjustL(.3f).ToString());
		}

		[Test]
		public void Darken()
		{
			/*
			// Copyright (c) 2006-2014 Hampton Catlin, Natalie Weizenbaum, and Chris Eppstein
			assert_equal("#ff6a00", evaluate("darken(hsl(25, 100, 80), 30%)"))
			assert_equal("#220000", evaluate("darken(#800, 20%)"))
			assert_equal("black", evaluate("darken(#000, 20%)"))
			assert_equal("black", evaluate("darken(#800, 100%)"))
			assert_equal("#880000", evaluate("darken(#800, 0%)"))
			assert_equal("rgba(34, 0, 0, 0.5)", evaluate("darken(rgba(136, 0, 0, 0.5), 20%)"))
			assert_equal("rgba(34, 0, 0, 0.5)", evaluate("darken($color: rgba(136, 0, 0, 0.5), $amount: 20%)"))
			*/

			// Assert.AreEqual("ff6a00", new Hsla(0.25f, 0.100f, 0.80f).AdjustLightness(-.3f).ToRgb().ToHex());
			Assert.AreEqual("220000", WebColor.Parse("#800").Darken(0.2f).ToHex());
			Assert.AreEqual("000000", WebColor.Parse("#000").Darken(0.2f).ToHex());
			Assert.AreEqual("000000", WebColor.Parse("#800").Darken(1f).ToHex());
			Assert.AreEqual("880000", WebColor.Parse("#800").Darken(0f).ToHex());
			Assert.AreEqual("rgba(34, 0, 0, 0.5)", WebColor.Parse("rgba(136, 0, 0, 0.5)").Darken(0.2f).ToString());

			// Assert.AreEqual("hsla(0, 0, 30, 1)", new Hsla(0d, 0d, 0d).AdjustL(.3f).ToString());
		}

		[Test]
		public void Saturate()
		{
			/*
			// Copyright (c) 2006-2014 Hampton Catlin, Natalie Weizenbaum, and Chris Eppstein
			assert_equal("#d9f2d9", evaluate("saturate(hsl(120, 30, 90), 20%)"))
			assert_equal("#9e3f3f", evaluate("saturate(#855, 20%)"))
			assert_equal("black", evaluate("saturate(#000, 20%)"))
			assert_equal("white", evaluate("saturate(#fff, 20%)"))
			assert_equal("#33ff33", evaluate("saturate(#8a8, 100%)"))
			assert_equal("#88aa88", evaluate("saturate(#8a8, 0%)"))
			assert_equal("rgba(158, 63, 63, 0.5)", evaluate("saturate(rgba(136, 85, 85, 0.5), 20%)"))
			assert_equal("rgba(158, 63, 63, 0.5)", evaluate("saturate($color: rgba(136, 85, 85, 0.5), $amount: 20%)"))
			*/

			// Assert.AreEqual("d9f2d9", new Hsla(120f / 360f, 0.3f, 0.9f).AdjustSaturation(0.2f).ToRgb().ToHex());
			Assert.AreEqual("9e3f3f", WebColor.Parse("#855").Saturate(0.2f).ToHex());
			Assert.AreEqual("000000", WebColor.Parse("#000").Saturate(0.2f).ToHex());
			Assert.AreEqual("ffffff", WebColor.Parse("#fff").Saturate(0.2f).ToHex());
			Assert.AreEqual("rgba(158, 63, 63, 0.5)", WebColor.Parse("rgba(136, 85, 85, 0.5)").Saturate(0.2f).ToString());




			Assert.AreEqual("726b6b", WebColor.Parse("#855").Desaturate(0.2f).ToHex());
		}

		[Test]
		public void HueTests()
		{

			Assert.AreEqual(0f, WebColor.Parse("#e00000").ToHsla().H);
			Assert.AreEqual(0f, WebColor.Parse("#fff").ToHsla().H);
			Assert.AreEqual(2.97709918f, WebColor.Parse("#3ec1be").ToHsla().H);
			Assert.AreEqual(350.869568f, WebColor.Parse("#5c000e").ToHsla().HueDegrees);


			var hsla = WebColor.Parse("#5c000e").ToHsla();

			Assert.AreEqual(-0.152173907f, hsla.H);


			// Assert.AreEqual(-0.152173862f, hsla.WithHueDegrees(350.869568f).H);

			Assert.AreEqual("5c000e", hsla.WithHueDegrees(350.869568f).ToRgb().ToHex());
		}

		[Test]
		public void AdjustHue()
		{
			/*
			assert_equal("#deeded", evaluate("adjust-hue(hsl(120, 30, 90), 60deg)"))
			assert_equal("#ededde", evaluate("adjust-hue(hsl(120, 30, 90), -60deg)"))
			assert_equal("#886a11", evaluate("adjust-hue(#811, 45deg)"))
			assert_equal("black", evaluate("adjust-hue(#000, 45deg)"))
			assert_equal("white", evaluate("adjust-hue(#fff, 45deg)"))
			assert_equal("#88aa88", evaluate("adjust-hue(#8a8, 360deg)"))
			assert_equal("#88aa88", evaluate("adjust-hue(#8a8, 0deg)"))
			assert_equal("rgba(136, 106, 17, 0.5)", evaluate("adjust-hue(rgba(136, 17, 17, 0.5), 45deg)"))
			assert_equal("rgba(136, 106, 17, 0.5)", evaluate("adjust-hue($color: rgba(136, 17, 17, 0.5), $degrees: 45deg)"))
			*/

			Assert.AreEqual("deeded", new Hsla(0f, 0.3f, 0.9f).WithHueDegrees(120f).RotateHue(60f).ToRgb().ToHex());
			Assert.AreEqual("ededde", new Hsla(0f, 0.3f, 0.9f).WithHueDegrees(120f).RotateHue(-60f).ToRgb().ToHex());
			Assert.AreEqual("886a11", WebColor.Parse("#811").ToHsla().RotateHue(45f).ToRgb().ToHex());


			// Assert.AreEqual("#88aa88", WebColor.Parse("#8a8").ToHsla().AdjustHue(1f).ToRgb().ToHex());
			Assert.AreEqual("88aa88", WebColor.Parse("#8a8").ToHsla().RotateHue(0f).ToRgb().ToHex());
		}

		[Test]
		public void MixColors()
		{
			/*
			def test_mix
				assert_equal("#7f007f", evaluate("mix(#f00, #00f)"))
				assert_equal("#7f7f7f", evaluate("mix(#f00, #0ff)"))
				assert_equal("#7f9055", evaluate("mix(#f70, #0aa)"))
				assert_equal("#3f00bf", evaluate("mix(#f00, #00f, 25%)"))
				assert_equal("rgba(63, 0, 191, 0.75)", evaluate("mix(rgba(255, 0, 0, 0.5), #00f)"))
				assert_equal("red", evaluate("mix(#f00, #00f, 100%)"))
				assert_equal("blue", evaluate("mix(#f00, #00f, 0%)"))
				assert_equal("rgba(255, 0, 0, 0.5)", evaluate("mix(#f00, transparentize(#00f, 1))"))
				assert_equal("rgba(0, 0, 255, 0.5)", evaluate("mix(transparentize(#f00, 1), #00f)"))
				assert_equal("red", evaluate("mix(#f00, transparentize(#00f, 1), 100%)"))
				assert_equal("blue", evaluate("mix(transparentize(#f00, 1), #00f, 0%)"))
				assert_equal("rgba(0, 0, 255, 0)", evaluate("mix(#f00, transparentize(#00f, 1), 0%)"))
				assert_equal("rgba(255, 0, 0, 0)", evaluate("mix(transparentize(#f00, 1), #00f, 100%)"))
				assert_equal("rgba(255, 0, 0, 0)", evaluate("mix($color1: transparentize(#f00, 1), $color2: #00f, $weight: 100%)"))
			  end
			*/

			Assert.AreEqual("7f007f", WebColor.Parse("#f00").BlendWith(WebColor.Parse("#00f"), 0.5f).ToHex());
			Assert.AreEqual("7f7f7f", WebColor.Parse("#f00").BlendWith(WebColor.Parse("#0ff"), 0.5f).ToHex());
			Assert.AreEqual("3f00bf", WebColor.Parse("#f00").BlendWith(WebColor.Parse("#00f"), 0.25f).ToHex());
		}
	

		[Test]
		public void Webcolor2()
		{
			var color = WebColor.Parse("#ff0000");

			Assert.AreEqual(255, color.R);


			var hsl = color.ToHsla();

			Assert.AreEqual("hsla(0,100%,50%,1)", hsl.ToString());
			Assert.AreEqual("hsla(0,100%,100%,1)", hsl.WithL(1).ToString());



			Assert.AreEqual("#ffffff", hsl.WithL(1).ToRgb().ToString());



			Assert.AreEqual("#ff0000", hsl.ToRgb().ToString());

		}
	}
}
