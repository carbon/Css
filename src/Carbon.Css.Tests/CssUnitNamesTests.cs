using Xunit;

namespace Carbon.Css.Tests
{
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
        }
    }
}