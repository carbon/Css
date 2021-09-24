namespace Carbon.Css.Tests;

public class CssBoxAlignmentTests
{
    [Fact]
    public void ValuesDontChange()
    {
        Assert.Equal(1, (byte)CssBoxAlignment.Start);
        Assert.Equal(2, (byte)CssBoxAlignment.End);
        Assert.Equal(3, (byte)CssBoxAlignment.Center);
    }

    [Fact]
    public void Canonicalize()
    {
        Assert.Equal("start",  CssBoxAlignment.Start.Canonicalize());
        Assert.Equal("end",    CssBoxAlignment.End.Canonicalize());
        Assert.Equal("center", CssBoxAlignment.Center.Canonicalize());

        // Baselines
        Assert.Equal("first baseline", CssBoxAlignment.FirstBaseline.Canonicalize());
        Assert.Equal("last baseline",  CssBoxAlignment.LastBaseline.Canonicalize());

        Assert.Equal("space-around",  CssBoxAlignment.SpaceAround.Canonicalize());
        Assert.Equal("space-between", CssBoxAlignment.SpaceBetween.Canonicalize());
        Assert.Equal("space-evenly",  CssBoxAlignment.SpaceEvenly.Canonicalize());

        // Overflow
        Assert.Equal("safe center",   CssBoxAlignment.SafeCenter.Canonicalize());
        Assert.Equal("unsafe center", CssBoxAlignment.UnsafeCenter.Canonicalize());
    }

    [Fact]
    public void CanonicalizeFlex()
    {
        Assert.Equal("flex-start", CssBoxAlignment.Start.Canonicalize(BoxLayoutMode.Flex));
        Assert.Equal("flex-end",   CssBoxAlignment.End.Canonicalize(BoxLayoutMode.Flex));
        Assert.Equal("center",     CssBoxAlignment.Center.Canonicalize(BoxLayoutMode.Flex));
    }
}