using System;
using Carbon.Color;
using Xunit;

namespace Carbon.Css.Tests
{
    public class CssPaddingTests
    {
        [Fact]
        public void A()
        {
            var a = CssPadding.Parse("10px 20%");

            Assert.Equal("10px", a.Top.ToString());
            Assert.Equal("20%", a.Left.ToString());
            Assert.Equal("10px", a.Bottom.ToString());
            Assert.Equal("20%", a.Right.ToString());
        }

        [Fact]
        public void B()
        {
            var a = CssPadding.Parse("10px");

            Assert.Equal("10px", a.Top.ToString());
            Assert.Equal("10px", a.Left.ToString());
            Assert.Equal("10px", a.Bottom.ToString());
            Assert.Equal("10px", a.Right.ToString());
        }
    }

    public class LinearGradientTests
    {
        [Fact]
        public void MozExamples()
        {
            LinearGradient.Parse("linear-gradient(#e66465, #9198e5)");
            // LinearGradient.Parse("linear-gradient(0.25turn, #3f87a6, #ebf8e1, #f69d3c);");

            LinearGradient.Parse("linear-gradient(to left, #333, #333 50%, #eee 75%, #333 75%)");

            // https://developer.mozilla.org/en-US/docs/Web/CSS/linear-gradient
        }

        [Fact]
        public void ThrowsOnInvalidInput()
        {
            Assert.Throws<ArgumentException>(() => LinearGradient.Parse(",,,,,,"));
            Assert.Throws<Exception>(() => LinearGradient.Parse("linear-gradientt(#000)"));
        }

        [Fact]
        public void ParseNamedColors()
        {
            var result = LinearGradient.Parse("linear-gradient(135deg, orange, orange 60%, cyan)");
            
            Assert.Equal(135d, result.Angle);

            Assert.Equal("ffa500ff", result.ColorStops[0].Color.ToHex8());
            Assert.Equal("ffa500ff", result.ColorStops[1].Color.ToHex8());
            Assert.Equal("00ffffff", result.ColorStops[2].Color.ToHex8());

            Assert.Equal("linear-gradient(135deg, #ffa500, #ffa500 60%, #00ffff)", result.ToString());
        }

        [Fact]
        public void ParseDefault()
        {
            var result = LinearGradient.Parse("linear-gradient(#000, transparent 50%, #fff)");

            Assert.Null(result.Angle);
            Assert.Equal(LinearGradientDirection.None, result.Direction);

            Assert.Equal("000000ff", result.ColorStops[0].Color.ToHex8());
            Assert.Equal("00000000", result.ColorStops[1].Color.ToHex8());
            Assert.Equal("ffffffff", result.ColorStops[2].Color.ToHex8());
        }

        [Fact]
        public void ParseFunctionSyntax()
        {
            var result = LinearGradient.Parse("linear-gradient(top left, #000, transparent 50%, #fff)");

            Assert.Null(result.Angle);
            Assert.Equal(LinearGradientDirection.TopLeft, result.Direction);


            Assert.Equal("000000ff", result.ColorStops[0].Color.ToHex8());
            Assert.Equal("00000000", result.ColorStops[1].Color.ToHex8());
            Assert.Equal("ffffffff", result.ColorStops[2].Color.ToHex8());
        }

        [Fact]
        public void ParseLegacySyntaxWithoutTo()
        {
            var result = LinearGradient.Parse("top left, #000, #00000000 50%, #fff");

            Assert.Null(result.Angle);
            Assert.Equal(LinearGradientDirection.TopLeft, result.Direction);


            Assert.Equal("000000ff", result.ColorStops[0].Color.ToHex8());
            Assert.Equal("00000000", result.ColorStops[1].Color.ToHex8());
            Assert.Equal("ffffffff", result.ColorStops[2].Color.ToHex8());
        }

        [Fact]
        public void ParseDirectional()
        {
            var result = LinearGradient.Parse("to top, #000, transparent 50%, #fff");

            Assert.Null(result.Angle);
            Assert.Equal(LinearGradientDirection.Top, result.Direction);

            Assert.Equal("000000ff", result.ColorStops[0].Color.ToHex8());
            Assert.Equal("00000000", result.ColorStops[1].Color.ToHex8());
            Assert.Equal("ffffffff", result.ColorStops[2].Color.ToHex8());
        }

        [Fact]
        public void ParseAngle()
        {
            var result = LinearGradient.Parse("30deg, #006, #00a 90%, #0000af 100%");

            Assert.Equal(3, result.ColorStops.Length);

            Assert.Equal("000066", result.ColorStops[0].Color.ToHex6());
            Assert.Equal("0000aa", result.ColorStops[1].Color.ToHex6());
            Assert.Equal("0000af", result.ColorStops[2].Color.ToHex6());

            Assert.Equal(30, result.Angle);
        }

        [Fact]
        public void ParseColorStop()
        {
            var stop = LinearColorStop.Parse("#00a 90%");

            Assert.Equal(Rgba32.Parse("#00a"), stop.Color);
            Assert.Equal(0.9, stop.Position);

        }
    }
}