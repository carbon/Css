namespace Carbon.Css.Tests;

public class CssPlacementTests
{
    [Fact]
    public void ParseValues()
    {
        var placement = CssPlacement.Parse("center start");

        // vertically centered, left justified
        Assert.Equal((CssBoxAlignment.Center, CssBoxAlignment.Start), (placement.Align, placement.Justify));

        Assert.Equal("center start", placement.ToString());
    }

    [Fact]
    public void ParseValue()
    {
        var placement = CssPlacement.Parse("center");

        Assert.Equal((CssBoxAlignment.Center, CssBoxAlignment.Center), (placement.Align, placement.Justify));

        Assert.Equal("center", placement.ToString());
    }

    [Fact]
    public void ParseValue2()
    {
        var placement = CssPlacement.Parse("end space-evenly");

        Assert.Equal((CssBoxAlignment.End, CssBoxAlignment.SpaceEvenly), (placement.Align, placement.Justify));

        Assert.Equal("end space-evenly", placement.ToString());
    }

    [Fact]
    public void ParseValue3()
    {
        var placement = CssPlacement.Parse("space-around start");

        Assert.Equal((CssBoxAlignment.SpaceAround, CssBoxAlignment.Start), (placement.Align, placement.Justify));

        Assert.Equal("space-around start", placement.ToString());
    }
}
