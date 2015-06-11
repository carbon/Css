namespace Carbon.Css
{
	using System;

	using Carbon.Css.Tests;

	using Xunit;

	public class ExpressionTests
	{
		[Fact]
		public void ExpressionTest1()
		{
			var sheet = StyleSheet.Parse(@"

$bgColor: orange;

@if $bgColor == orange { 
  div {
    background-color: $bgColor;
  }
}
");


			Assert.Equal("div { background-color: orange; }", sheet.ToString());
		}

		[Fact]
		public void ExpressionTest2()
		{
			var sheet = StyleSheet.Parse(@"


$bgColor: #ffffff;

@if rgba($bgColor, 0.5) == rgba(255, 255, 255, 0.5) { 
  div {
    background-color: $bgColor;
  }
}
");


			Assert.Equal("div { background-color: #ffffff; }", sheet.ToString());
		}

		[Fact]
		public void ExpressionTest3()
		{
			var sheet = StyleSheet.Parse(@"
$backgroundColor: #ffffff;
$foregroundColor: #000000;

@if readability($foregroundColor, $backgroundColor) > 0.5 { 
  div {
    background-color: red;
  }
}
");

			Assert.Equal("div { background-color: red; }", sheet.ToString());
		}

	}
}
