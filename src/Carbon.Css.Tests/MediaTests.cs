
using Xunit;

namespace Carbon.Css.Tests
{
    public class MediaTests
    {
        [Fact]
        public void RgbaTest()
        {
            var sheet = StyleSheet.Parse("div { color: rgba(100, 100, 100, 0.5); }");

            Assert.Equal("div { color: rgba(100, 100, 100, 0.5); }", sheet.ToString());
        }

        [Fact]
        public void MediaMixin1()
        {
            var sheet = StyleSheet.Parse(@"
@mixin hi { 
  color: red;
}

@mixin blerg { 
	a {
		color: pink;

		&:hover { color: #000; }
	}
}

@media (min-width: 700px) { 
	@include blerg;

	div { 
		background-color: $bgColor;
		@include hi;
	}
}
");

            Assert.Equal(2, sheet.Context.Mixins.Count);


            Assert.True(sheet.Context.Mixins.ContainsKey("hi"));
            Assert.True(sheet.Context.Mixins.ContainsKey("blerg"));

            Assert.Single(sheet.Children);

            var rule = sheet.Children[0] as MediaRule;

            Assert.NotNull(rule);

            Assert.Equal("(min-width: 700px)", rule.Queries.ToString());


            var include = rule.Children[0] as IncludeNode;

            Assert.Equal("blerg", include.Name);
        }

        [Fact]
        public void WithMixin()
        {
            var sheet = StyleSheet.Parse(@"

$bgColor: orange;

@mixin hi { 
	color: red;
}

@mixin blerg { 
	a {
		color: pink;

		&:hover { color: #000; }
	}
}

@media (min-width: 700px) { 
	@include blerg;

	div { 
		background-color: $bgColor;
		@include hi;
	}
}
");


            Assert.Equal(@"@media (min-width: 700px) {
  a { color: pink; }
  a:hover { color: #000; }
  div {
    background-color: orange;
    color: red;
  }
}", sheet.ToString());
        }

    }
}
