using Xunit;

namespace Carbon.Css.Tests
{
    public class EachTests
    {
        [Fact]
        public void A_to()
        {
            var sheet = StyleSheet.Parse(@"

$sizes: 40px, 50px, 80px;

@each $size in $sizes { 
  .icon-#{$size} { font-size: $size; }
}
");

            Assert.Equal(@"
.icon-40px { font-size: 40px; }
.icon-50px { font-size: 50px; }
.icon-80px { font-size: 80px; }".Trim(), sheet.ToString());
        }

        /*
        [Fact]
        public void EachMap()
        {
            var sheet = StyleSheet.Parse(@"

$sizes: (""a"": ""b"", ""c"": ""d"");

@each $size in $sizes { 
  .icon-#{$size} { font-size: $size; }
}
");

            Assert.Equal(@"
.icon-40px { font-size: 40px; }
.icon-50px { font-size: 50px; }
.icon-80px { font-size: 80px; }".Trim(), sheet.ToString());
        }
        */
    }
}
