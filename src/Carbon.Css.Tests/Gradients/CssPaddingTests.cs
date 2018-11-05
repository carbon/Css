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
}