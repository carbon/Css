using Xunit;

namespace Carbon.Css.Tests
{
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
        }

        [Fact]
        public void B()
        {
            var edge = Thickness.Parse("1 2 3 4");

            Assert.Equal("1", edge.Top.ToString());
            Assert.Equal("2", edge.Left.ToString());
            Assert.Equal("3", edge.Bottom.ToString());
            Assert.Equal("4", edge.Right.ToString());
        }

        [Fact]
        public void C()
        {
            var a = Thickness.Parse("10px 20%");

            Assert.Equal("10px", a.Top.ToString());
            Assert.Equal("20%", a.Left.ToString());
            Assert.Equal("10px", a.Bottom.ToString());
            Assert.Equal("20%", a.Right.ToString());

            Assert.Equal("10px 20% 10px 20%", a.ToString());
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
    }
}