namespace Carbon.Css.Tests
{
    public class CssValueTests
    {
        [Fact]
        public void Q()
        {
            var value = CssUnitValue.Parse("97.916666666666666666666666666667%");

            Assert.Equal(97.916666666666666666666666666667d, value.Value);
        }

        [Fact]
        public void NumbersAreStoredAtDoublePrecision()
        {
            string text = "97.916666666666666666666666666667%";

            var value = CssValue.Parse(text) as CssUnitValue;

            Assert.Equal(97.916666666666666666666666666667d, value.Value);
            Assert.Equal(97.916666666666666666666666666667d, CssUnitValue.Parse(text).Value);

        }

        [Fact]
        public void ParseValues()
        {
            var (value, unit) = (CssUnitValue)CssValue.Parse("14px");

            Assert.Equal(14f, value);
            Assert.Equal("px", unit.Name);
        }

        [Fact]
        public void ParseValues2()
        {
            var (value, unit) = CssUnitValue.Parse("14px");

            Assert.Equal(14f, value);
            Assert.Equal("px", unit.Name);
        }

        [Fact]
        public void ParsePx()
        {
            Assert.Equal("14px", CssValue.Parse("14px").ToString());
            Assert.Equal("14px", CssUnitValue.Parse("14px").ToString());
        }

        [Fact]
        public void A()
        {
            Assert.Equal("left", CssValue.Parse("left").ToString());
        }

        [Fact]
        public void Px()
        {
            var (value, unit) = CssUnitValue.Parse("12px");

            Assert.Equal(12d, value);
            Assert.Same(CssUnitInfo.Px, unit);
        }

        [Fact]
        public void Em()
        {
            var (value, unit) = CssUnitValue.Parse("50em");

            Assert.Equal(50d, value);
            Assert.Same(CssUnitInfo.Em, unit);
        }

        [Fact]
        public void Percent()
        {
            Assert.Equal("50%", CssValue.Parse("50%").ToString());
            Assert.Equal("50%", CssUnitValue.Parse("50%").ToString());
        }

        [Fact]
        public void ValueList()
        {
            var value = CssValue.Parse("100px 100px 100px 100px") as CssValueList;

            Assert.Equal(4, value.Count);
            Assert.Equal("100px 100px 100px 100px", value.ToString());
        }

      


        [Fact]
        public void Url()
        {
            Assert.Equal("hi.jpeg", CssUrlValue.Parse("url(hi.jpeg)").Value);
            Assert.Equal("hi.jpeg", CssUrlValue.Parse("url('hi.jpeg')").Value);
        }
    }
}