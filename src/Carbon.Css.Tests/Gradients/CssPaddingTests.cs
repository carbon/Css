using Xunit;

namespace Carbon.Css.Tests
{
    public class ThicknessTests
    {
        [Fact]
        public void A()
        {
            var a = Thickness.Parse("10px 20%");

            Assert.Equal("10px", a.Top.ToString());
            Assert.Equal("20%", a.Left.ToString());
            Assert.Equal("10px", a.Bottom.ToString());
            Assert.Equal("20%", a.Right.ToString());

            Assert.Equal("10px 20% 10px 20%", a.ToString());
        }

        [Fact]
        public void B()
        {
            var a = Thickness.Parse("10px");

            Assert.Equal("10px", a.Top.ToString());
            Assert.Equal("10px", a.Left.ToString());
            Assert.Equal("10px", a.Bottom.ToString());
            Assert.Equal("10px", a.Right.ToString());

            Assert.Equal("10px", a.ToString());
        }
    }
}