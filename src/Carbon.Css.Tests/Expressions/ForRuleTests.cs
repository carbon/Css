using Xunit;

namespace Carbon.Css.Tests
{
    public class ForRuleTests
    {

        [Fact]
        public void A()
        {
            var sheet = StyleSheet.Parse(@"
@for $i from 1 through 5 { 
  div { width: #{$i}px }
}
");

            Assert.Equal(@"
div { width: 1px; }
div { width: 2px; }
div { width: 3px; }
div { width: 4px; }
div { width: 5px; }".Trim(), sheet.ToString());
        }

        [Fact]
        public void B()
        {
            var sheet = StyleSheet.Parse(@"
@for $i from 1 through 5 { 
    .col-#{$i} { width: #{$i}px }
}
");

            Assert.Equal(@"
div { width: 1px; }
div { width: 2px; }
div { width: 3px; }
div { width: 4px; }
div { width: 5px; }".Trim(), sheet.ToString());
        }
    }
}
