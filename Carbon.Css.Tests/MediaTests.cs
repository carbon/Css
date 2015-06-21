namespace Carbon.Css.Tests
{
	using Xunit;

	public class MediaTests : FixtureBase
	{
		[Fact]
		public void RgbaTest()
		{
			var sheet = StyleSheet.Parse("div { color: rgba(100, 100, 100, 0.5); }");

			Assert.Equal("div { color: rgba(100, 100, 100, 0.5); }", sheet.ToString());
        }

        [Fact]
		public void MediaTest1()
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
