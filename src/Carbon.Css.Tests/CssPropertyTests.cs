namespace Carbon.Css.Tests
{
    public class CssPropertyTests
    {
        [Fact]
        public void Common()
        {
            Assert.Equal(CssProperty.Display, CssProperty.Get("display"));
            Assert.Equal(CssProperty.Width,   CssProperty.Get("width"));
        }

        // clip-path, clip-rule and mask properties [css-masking-1]

        [Fact]
        public void Clipping()
        {
            Assert.Equal(CssProperty.ClipPath,      CssProperty.Get("clip-path"));
            Assert.Equal(CssProperty.ClipRule,      CssProperty.Get("clip-rule"));
            Assert.Equal(CssProperty.StrokeLinecap, CssProperty.Get("stroke-linecap"));
            Assert.Equal(CssProperty.StrokeWidth,   CssProperty.Get("stroke-width"));
        }

        [Fact]
        public void Svg()
        {
            Assert.Equal(CssProperty.Fill,          CssProperty.Get("fill"));
            Assert.Equal(CssProperty.Stroke,        CssProperty.Get("stroke"));
            Assert.Equal(CssProperty.StrokeLinecap, CssProperty.Get("stroke-linecap"));
            Assert.Equal(CssProperty.StrokeWidth,   CssProperty.Get("stroke-width"));
        }
        
    }
}