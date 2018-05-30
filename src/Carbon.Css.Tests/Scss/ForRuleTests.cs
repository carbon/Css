using System.Collections.Generic;
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
.col-1 { width: 1px; }
.col-2 { width: 2px; }
.col-3 { width: 3px; }
.col-4 { width: 4px; }
.col-5 { width: 5px; }".Trim(), sheet.ToString());
        }


        [Fact]
        public void C()
        {
            var dic = new Dictionary<string, CssValue>
            {
                ["columnCount"] = CssValue.Parse("5"),
                ["gap"] = CssValue.Parse("10px")
            };

            var sheet = StyleSheet.Parse(@"
@for $i from 1 through $columnCount { 
  .col-#{$i} { width: #{$i}px; margin: $gap; }
}
");
            

            Assert.Equal(@"
.col-1 {
  width: 1px;
  margin: 10px;
}
.col-2 {
  width: 2px;
  margin: 10px;
}
.col-3 {
  width: 3px;
  margin: 10px;
}
.col-4 {
  width: 4px;
  margin: 10px;
}
.col-5 {
  width: 5px;
  margin: 10px;
}

".Trim(), sheet.ToString(dic));
        }
    }
}
