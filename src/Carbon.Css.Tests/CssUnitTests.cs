namespace Carbon.Css.Tests
{
    public class CssUnitTests
    {
        // switch = 224ms
        // dictionary = 149ms

        [Fact]
        public void Lookup()
        {
            Assert.Equal(CssUnitInfo.Px,  CssUnitInfo.Get("px"));
            Assert.Equal(CssUnitInfo.Pt,  CssUnitInfo.Get("pt"));
            Assert.Equal(CssUnitInfo.Deg, CssUnitInfo.Get("deg"));
            Assert.Equal(CssUnitInfo.Rlh, CssUnitInfo.Get("rlh"));
            Assert.Equal(CssUnitInfo.Rem, CssUnitInfo.Get("rem"));
        }

        [Fact]
        public void Equality()
        {
            Assert.True(CssUnitInfo.Px.Equals(CssUnitInfo.Px));
            Assert.False(CssUnitInfo.Pt.Equals(CssUnitInfo.Px));
        }

        [Theory]
        [InlineData("px", NodeKind.Length)]
        [InlineData("pt", NodeKind.Length)]
        [InlineData("kHz", NodeKind.Frequency)]
        [InlineData("s", NodeKind.Time)]
        public void KindIsCorrect(string text, NodeKind kind)
        {
            Assert.Equal(kind, CssUnitInfo.Get(text).Kind);
        }

        [Fact]
        public void UnknownUnitEquality()
        {
            Assert.True(CssUnitInfo.Get("ns").Equals(CssUnitInfo.Get("ns")));
            Assert.False(CssUnitInfo.Get("ns").Equals(CssUnitInfo.Get("ms")));
        }
    }
}