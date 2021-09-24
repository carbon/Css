namespace Carbon.Css.Tests
{
    public class ColorTests
    {
        [Fact]
        public void RgbaTest()
        {
            var sheet = StyleSheet.Parse("div { color: rgba(100, 100, 100, 0.5); }");

            Assert.Equal("div { color: rgba(100, 100, 100, 0.5); }", sheet.ToString());
        }
    }
}