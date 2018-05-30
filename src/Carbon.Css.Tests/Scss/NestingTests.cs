using Xunit;

namespace Carbon.Css.Tests
{
    public class NestingTests
    {
        [Fact]
        public void A()
        {
            var css = StyleSheet.Parse("div { &.hide { display: none; } }");

            Assert.Equal("div.hide { display: none; }", css.ToString());
        }

        /*
        [Fact]
        public void D()
        {
            var css = StyleSheet.Parse("div, span { &.hide { display: none; } }");

            // div &.hide,\r\nspan &.hide { display: none; }

            Assert.Equal("div.hide { display: none; }", css.ToString());
        }
        */

        [Fact]
        public void E()
        {
            var css = StyleSheet.Parse(@"div, span { .hide { display: none; } }");

            // div &.hide,\r\nspan &.hide { display: none; }

            Assert.Equal(@"
div .hide,
span .hide { display: none; }".Trim(), css.ToString());
        }

        /*
        [Fact]
        public void B()
        {
            var css = StyleSheet.Parse("div { &.hide, &.hidden { display: none; } }");

            Assert.Equal("div.hide, div.hidden { display: none; }", css.ToString());
        }
        */

        [Fact]
        public void C()
        {
            var css = StyleSheet.Parse(".hide { body & { display: none; } }");

            Assert.Equal("body .hide { display: none; }", css.ToString());
        }
    }
}