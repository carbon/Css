using Carbon.Color;

namespace Carbon.Css.Tests;

public class CssColorTests
{
    [Theory]
    [InlineData("hsl(120deg 100% 50%)",         "#0f0")] // green
    [InlineData("hsl(0deg 100% 50%)",           "#f00")] // red
    [InlineData("hsl(240deg 100% 50%)",         "#00f")] // blue
    [InlineData("hsla(240deg 100% 50% / 100%)", "#00f")] // blue
    public void CanParseHsl(string text, string expected)
    {
        var color = CssColor.Parse(text);

        Assert.Equal(expected, color.Value.ToRgba32().ToHexString());
    }
  
    [Fact]
    public void CanParseRgb()
    {
        for (int i = 0; i < 100; i++)
        {
            var r = (byte)Random.Shared.Next(0, 255);
            var g = (byte)Random.Shared.Next(0, 255);
            var b = (byte)Random.Shared.Next(0, 255);

            var rgb = new Rgba32(r, g, b);

            var c1 = CssColor.Parse($"rgb({r},{g},{b})");
            var c2 = CssColor.Parse($"rgb({r} {g} {b} / 100%)");
            var c3 = CssColor.Parse($"rgb({r} {g} {b})");
            var c4 = CssColor.Parse($"rgb({r} {g} {b} / 1)");
            var c5 = CssColor.Parse($"rgba({r} {g} {b} / 1)");

            Assert.Equal(rgb, c1.Value.ToRgba32());
            Assert.Equal(rgb, c2.Value.ToRgba32());
            Assert.Equal(rgb, c3.Value.ToRgba32());
            Assert.Equal(rgb, c4.Value.ToRgba32());
            Assert.Equal(rgb, c5.Value.ToRgba32());
        }
    }
}


// color(rec2020 0.42053 0.979780 0.00579)

// color(display-p3 34% 58% 73%)
// color(display-p3 .34 .58 .73)
// color(display-p3 34% 58% 73% / 50%)
// color(display-p3 .34 .58 .73 / .5)
// color(display-p3 100% 100% 100%)
// color(display-p3 1 1 1)
// color(display-p3 0% 0% 0%)
// color(display-p3 0 0 0)
// color(display-p3 none none none)
// color(display-p3 0)
// color(display-p3)

// color(xyz-d65 22% 26% 53%)
// color(xyz-d65 .22 .26 .53)
// color(xyz-d65 .22 .26 .53 / 50%)
// color(xyz-d65 .22 .26 .53 / .5)
// color(xyz-d65 100% 100% 100%)
// color(xyz-d65 1 1 1)
// color(xyz-d65 0% 0% 0%)
// color(xyz-d65 0 0 0)
// color(xyz-d65 none none none)
// color(xyz-d65)


// | rgb(146.064 107.457 131.223)
// | rgba
// | hsl
// | hsla
// | hwb
// | lab(29.69% 44.888% -29.04%)
// | lch(60.2345 59.2 95.2)
// | oklab(40.101% 0.1147 0.0453)
// | oklch(78.3% 0.108 326.5)


// https://codepen.io/argyleink/pen/RwyOyeq
// https://www.bram.us/2020/04/27/colors-in-css-hello-space-separated-functional-color-notations/