
using Xunit;

namespace Carbon.Css.Tests
{
    public class CssGapTests
    {
        [Fact]
        public void Construct()
        {
            var gap = new CssGap(CssUnitValue.Parse("10%"));

            Assert.Equal("10%", gap.X.ToString());
            Assert.Equal("10%", gap.Y.ToString());
        }

        [Fact]
        public void ParseSingleValue()
        {
            var gap = CssGap.Parse("10px");

            Assert.Equal("10px", gap.X.ToString());
            Assert.Equal("10px", gap.Y.ToString());
        }

        [Fact]
        public void ParseDecimalValue()
        {
            var gap = CssGap.Parse("10.5rem");

            Assert.Equal("10.5rem", gap.X.ToString());
            Assert.Equal("10.5rem", gap.Y.ToString());
        }

        [Fact]
        public void ParseDoubleValue()
        {
            var gap = CssGap.Parse("10px 20px");

            Assert.Equal("10px", gap.X.ToString());
            Assert.Equal("20px", gap.Y.ToString());
        }
    }
}