namespace Carbon.Css.Tests;

public class CssUnitNamesTests
{
    [Fact]
    public void AreSame()
    {
        Assert.Same(CssUnitNames.Hz,   CssUnitNames.Get("Hz"));
        Assert.Same(CssUnitNames.Em,   CssUnitNames.Get("em"));
        Assert.Same(CssUnitNames.Px,   CssUnitNames.Get("px"));
        Assert.Same(CssUnitNames.Vmin, CssUnitNames.Get("vmin"));
        Assert.Same(CssUnitNames.Vmax, CssUnitNames.Get("vmax"));

        // Safari 15+
        Assert.Same(CssUnitNames.Svw, CssUnitNames.Get("svw"));
        Assert.Same(CssUnitNames.Svh, CssUnitNames.Get("svh"));
        Assert.Same(CssUnitNames.Svi, CssUnitNames.Get("svi"));
        Assert.Same(CssUnitNames.Svb, CssUnitNames.Get("svb"));

        Assert.Same(CssUnitNames.Lvw, CssUnitNames.Get("lvw"));
        Assert.Same(CssUnitNames.Lvh, CssUnitNames.Get("lvh"));
        Assert.Same(CssUnitNames.Lvi, CssUnitNames.Get("lvi"));
        Assert.Same(CssUnitNames.Lvb, CssUnitNames.Get("lvb"));


        Assert.Same(CssUnitNames.Vi, CssUnitNames.Get("vi"));
        Assert.Same(CssUnitNames.Vb, CssUnitNames.Get("vb"));



        // svmin, svmax
    }
}