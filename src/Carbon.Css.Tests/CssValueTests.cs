using Xunit;

namespace Carbon.Css.Tests
{
    public class CssValueTests
    {
        [Fact]
        public void A()
        {
            Assert.Equal("left", CssValue.Parse("left").ToString());
        }

        [Fact]
        public void Px()
        {
            Assert.Equal("50px", CssValue.Parse("50px").ToString());
        }

        [Fact]
        public void Percent()
        {
            Assert.Equal("50%", CssValue.Parse("50%").ToString());
        }

        [Fact]
        public void ValueList()
        {
            var value = CssValue.Parse("100px 100px 100px 100px") as CssValueList;

            Assert.Equal(4, value.Count);
            Assert.Equal("100px 100px 100px 100px", value.ToString());
        }

        [Fact]
        public void ParseValues()
        {
            var (value, unit) = CssValue.Parse("14px") as CssUnitValue;

            Assert.Equal(14f, value);
            Assert.Equal("px", unit.Name);
        }


        [Fact]
        public void Url()
        {
            Assert.Equal("hi.jpeg", CssUrlValue.Parse("url(hi.jpeg)").Value);
            Assert.Equal("hi.jpeg", CssUrlValue.Parse("url('hi.jpeg')").Value);
        }
    }
}
