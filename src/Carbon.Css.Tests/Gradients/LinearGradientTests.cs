#pragma warning disable IDE0057 // Use range operator
#pragma warning disable IDE0018 // Inline variable declaration

using Carbon.Color;

namespace Carbon.Css.Gradients.Tests;

public class LinearGradientTests
{
    [Fact]
    public void NegitiveAngle()
    {
        var gradient = LinearGradient.Parse("linear-gradient(-30deg, rgb(153, 116, 186) 0%, rgb(194, 234, 9) 20%, rgb(0, 203, 227) 80%, rgb(47, 84, 176) 100%)");

        Assert.Equal(-30d, gradient.Angle.Value);

        Assert.Equal(new Rgba32(153, 116, 186), gradient.Stops[0].Color);
        Assert.Equal(new Rgba32(194, 234, 9), gradient.Stops[1].Color);

        Assert.Equal(4, gradient.Stops.Length);

        Assert.Equal("linear-gradient(-30deg, #9974ba 0%, #c2ea09 20%, #00cbe3 80%, #2f54b0 100%)", gradient.ToString());
        Assert.Equal("linear-gradient(-30deg, #9974ba 0%, #c2ea09 20%, #00cbe3 80%, #2f54b0 100%)", LinearGradient.Parse(gradient.ToString()).ToString());

    }

    [Fact]
    public void A()
    {
        var gradient = LinearGradient.Parse("linear-gradient(0deg, rgb(153, 116, 186) 0%, rgb(194, 234, 9) 20%, rgb(0, 203, 227) 80%, rgb(47, 84, 176) 100%)");

        Assert.Equal(new Rgba32(153, 116, 186), gradient.Stops[0].Color);
        Assert.Equal(new Rgba32(194, 234, 9), gradient.Stops[1].Color);

        Assert.Equal(4, gradient.Stops.Length);
    }

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
        Assert.Throws<IndexOutOfRangeException>(() => LinearGradient.Parse(",,,,,,"));
        Assert.Throws<FormatException>(() => LinearGradient.Parse("linear-gradientt(#000)"));
    }

    [Fact]
    public void ParseNamedColors()
    {
        var result = LinearGradient.Parse("linear-gradient(135deg, orange, orange 60%, cyan)");

        Assert.Equal(135d, result.Angle);

        Assert.Equal("ffa500ff", result.Stops[0].Color.ToHex8());
        Assert.Equal("ffa500ff", result.Stops[1].Color.ToHex8());
        Assert.Equal("00ffffff", result.Stops[2].Color.ToHex8());

        Assert.Equal("linear-gradient(135deg, #ffa500, #ffa500 60%, #0ff)", result.ToString());
    }

    [Fact]
    public void ParseDefault()
    {
        var result = LinearGradient.Parse("linear-gradient(#000, transparent 50%, #fff)");

        Assert.Null(result.Angle);
        Assert.Equal(LinearGradientDirection.None, result.Direction);

        Assert.Equal("000000ff", result.Stops[0].Color.ToHex8());
        Assert.Equal("00000000", result.Stops[1].Color.ToHex8());
        Assert.Equal("ffffffff", result.Stops[2].Color.ToHex8());
    }

    [Fact]
    public void ParseFunctionSyntax()
    {
        var result = LinearGradient.Parse("linear-gradient(top left, #000, transparent 50%, #fff)");

        Assert.Null(result.Angle);
        Assert.Equal(LinearGradientDirection.TopLeft, result.Direction);


        Assert.Equal("000000ff", result.Stops[0].Color.ToHex8());
        Assert.Equal("00000000", result.Stops[1].Color.ToHex8());
        Assert.Equal("ffffffff", result.Stops[2].Color.ToHex8());
    }

    [Fact]
    public void ParseLegacySyntaxWithoutTo()
    {
        var result = LinearGradient.Parse("top left, #000, #00000000 50%, #fff");

        Assert.Null(result.Angle);
        Assert.Equal(LinearGradientDirection.TopLeft, result.Direction);


        Assert.Equal("000000ff", result.Stops[0].Color.ToHex8());
        Assert.Equal("00000000", result.Stops[1].Color.ToHex8());
        Assert.Equal("ffffffff", result.Stops[2].Color.ToHex8());
    }

    [Fact]
    public void ParseDirectional()
    {
        var result = LinearGradient.Parse("to top, #000, transparent 50%, #fff");

        Assert.Null(result.Angle);
        Assert.Equal(LinearGradientDirection.Top, result.Direction);

        Assert.Equal("000000ff", result.Stops[0].Color.ToHex8());
        Assert.Equal("00000000", result.Stops[1].Color.ToHex8());
        Assert.Equal("ffffffff", result.Stops[2].Color.ToHex8());
    }

    [Fact]
    public void ParseAngle()
    {
        var result = LinearGradient.Parse("30deg, #006, #00a 90%, #0000af 100%");

        Assert.Equal(3, result.Stops.Length);

        Assert.Equal("000066", result.Stops[0].Color.ToHex6());
        Assert.Equal("0000aa", result.Stops[1].Color.ToHex6());
        Assert.Equal("0000af", result.Stops[2].Color.ToHex6());

        Assert.Equal(30, result.Angle);
    }

    [Fact]
    public void ParseColorStop()
    {
        var stop = ColorStop.Parse("#00a 90%");

        Assert.Equal(Rgba32.Parse("#00a"), stop.Color);
        Assert.Equal(0.9, stop.Position);
    }

    [Fact]
    public void ParseColorStopWithoutValue()
    {
        var stop = ColorStop.Parse("#00a, #fff, #000");

        Assert.Equal(Rgba32.Parse("#00a"), stop.Color);
        Assert.Null(stop.Position);
    }

    [Fact]
    public void ParseColorStopList()
    {
        string text = "#00a 90%, #000 91%, #fff 92%";

        int read;

        var stop1 = ColorStop.Read(text, out read);

        Assert.Equal(8, read);

        var stop2 = ColorStop.Read(text.AsSpan(read + 2), out read);

        Assert.Equal(8, read);

        Assert.Equal(Rgba32.Parse("#00a"), stop1.Color);
        Assert.Equal(0.9, stop1.Position);

        Assert.Equal(Rgba32.Parse("#000"), stop2.Color);

    }

    [Fact]
    public void ParseColorStopList2()
    {
        var text = "rgb(153, 116, 186) 0%, rgb(194, 234, 9) 20%".AsSpan();

        ColorStop stop;

        stop = ColorStop.Read(text, out int read);

        Assert.Equal(21, read);

        Assert.Equal(0, stop.Position);

        stop = ColorStop.Read(text.Slice(read + 2), out read);

        Assert.Equal(20, read);

        Assert.Equal(0.2, stop.Position);
    }

    [Fact]
    public void ParseColorStopWithRgb()
    {
        var stop = ColorStop.Parse("rgb(153, 116, 186) 0%");

        Assert.Equal(new Rgba32(153, 116, 186), stop.Color);
        Assert.Equal(0, stop.Position);
    }
}
