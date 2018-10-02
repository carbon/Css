using Xunit;


namespace Carbon.Css.Tests
{
    public class CssFunctionTests
    {
        [Fact]
        public void ParseFunctionWithLeadingWhitespaceInArgument()
        {
            var sheet = StyleSheet.Parse("div { background-color: rgba( 42, 45, 53, 0.7); }");

            Assert.Equal("div { background-color: rgba(42, 45, 53, 0.7); }", sheet.ToString());
        }
    }
}