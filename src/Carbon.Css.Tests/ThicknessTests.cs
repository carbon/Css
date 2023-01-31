using System.Text.Json;

namespace Carbon.Css.Tests;

public class ThicknessTests
{
    [Fact]
    public void A()
    {
        var edge = Thickness.Parse("100px 200px");

        Assert.Equal("100px", edge.Top.ToString());
        Assert.Equal("100px", edge.Bottom.ToString());
        Assert.Equal("200px", edge.Left.ToString());
        Assert.Equal("200px", edge.Right.ToString());

        Assert.Equal("100px 200px", edge.ToString());
        Assert.Equal("100px 200px", $"{edge}");

        Assert.Equal(edge, JsonSerializer.Deserialize<Thickness>("\"100px 200px\""));
    }

    [Fact]
    public void TryParse_Sucesss()
    {
        Assert.True(Thickness.TryParse("100px 200px", out Thickness edge));

        Assert.Equal("100px 200px", edge.ToString());
    }

    [Fact]
    public void TryParse_False()
    {
        Assert.False(Thickness.TryParse("a", out _));
        Assert.False(Thickness.TryParse("", out _));
    }

    [Fact]
    public void B()
    {
        var edge = Thickness.Parse("1 2 3 4");

        Assert.Equal("1", edge.Top.ToString());
        Assert.Equal("2", edge.Left.ToString());
        Assert.Equal("3", edge.Bottom.ToString());
        Assert.Equal("4", edge.Right.ToString());

        Assert.Equal("1 2 3 4", edge.ToString());
        Assert.Equal("1 2 3 4", $"{edge}");
    }

    [Fact]
    public void C()
    {
        var a = Thickness.Parse("10px 20%");

        Assert.Equal("10px", a.Top.ToString());
        Assert.Equal("20%", a.Left.ToString());
        Assert.Equal("10px", a.Bottom.ToString());
        Assert.Equal("20%", a.Right.ToString());

        Assert.Equal("10px 20%", a.ToString());
    }

    [Fact]
    public void ParseZero()
    {
        var value = Thickness.Parse("0");

        Assert.Same(Thickness.Zero, value);

        Assert.Same(CssUnitValue.Zero, value.Top);
        Assert.Same(CssUnitValue.Zero, value.Left);
        Assert.Same(CssUnitValue.Zero, value.Right);
        Assert.Same(CssUnitValue.Zero, value.Bottom);

        Assert.Equal("0", value.ToString());
    }

    [Fact]
    public void D()
    {
        var a = Thickness.Parse("10px");

        Assert.Equal("10px", a.Top.ToString());
        Assert.Equal("10px", a.Left.ToString());
        Assert.Equal("10px", a.Bottom.ToString());
        Assert.Equal("10px", a.Right.ToString());

        Assert.Equal("10px", a.ToString());
    }

    [Fact]
    public void E()
    {
        var _10px = CssUnitValue.Parse("10px");

        var a = Thickness.Parse("10px 10px 10px 10px");

        Assert.Equal(_10px, a.Top);
        Assert.Equal(_10px, a.Left);
        Assert.Equal(_10px, a.Bottom);
        Assert.Equal(_10px, a.Right);

        Assert.Equal("10px", a.ToString());        
    }

    [Fact]
    public void ThreeVariables()
    {
        // When three values are specified, the first padding applies to the top, the second to the right and left, the third to the bottom.

        var edge = Thickness.Parse("169px 0px 174px");

        Assert.Equal("169px", edge.Top.ToString());
        Assert.Equal("0px", edge.Left.ToString());
        Assert.Equal("174px", edge.Bottom.ToString());
        Assert.Equal("0px", edge.Right.ToString());

        Assert.Same(edge.Left, edge.Right);

        Assert.Equal("169px 0px 174px", edge.ToString());
    }
}
