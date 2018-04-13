namespace Carbon.Css.Tests
{
    using Xunit;

    public class ParserTests
    {
        [Fact]
        public void ParseFunctionWithLeadingWhitespaceInArgument()
        {
            var sheet = StyleSheet.Parse("div { background-color: rgba( 42, 45, 53, 0.7); }");

            Assert.Equal("div { background-color: rgba(42, 45, 53, 0.7); }", sheet.ToString());
        }

        [Fact]
        public void ParsePartialDirective()
        {
            var sheet = StyleSheet.Parse(@"
//= partial
div { color: blue; }
"); // Prevents standalone compilation

            Assert.Equal("div { color: blue; }", sheet.ToString());
        }
    }
}