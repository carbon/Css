using Xunit;

namespace Carbon.Css
{
	public class ExpressionTests
	{
		[Fact]
		public void ExpressionTest7()
		{
			var sheet = StyleSheet.Parse(@"

$bgColor: #ffffff;

@if rgba($bgColor, 0.5) == rgba(255, 255, 255, 0.5) { 
  div {
    color: darken($bgColor, 50%);
    background-color: darken($bgColor, 0.5);
  }
}
");
		
			Assert.Equal(
@"div {
  color: #808080;
  background-color: #808080;
}", sheet.ToString());
		}

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

        /*
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
        */

        /*
        
        [Fact]
		public void ExpressionTest4()
		{
			var sheet = StyleSheet.Parse(@"
$backgroundColor: #ffffff;
$foregroundColor: #000000;


div {
  background-color: red;

  @if readability($foregroundColor, $backgroundColor) > 0.5 { 
	color: orange;
  }
}
");
			var ifBlock = (IfBlock)((CssRule)sheet.Children[2]).Children[1];

            Assert.Equal(NodeKind.If, ifBlock.Kind);
			Assert.Equal(ifBlock.Children.Count, 1);

			Assert.Equal(
@"div {
  background-color: red;
  color: orange;
}", sheet.ToString());
		}
        */

        [Fact]
        public void ExpressionTest5()
        {
            var sheet = StyleSheet.Parse(@"
$bgColor: #ffffff;

@if rgba($bgColor, 0.5) == rgba(255, 255, 255, 0.5) { 
  $bgColor: purple;  
}

div {
  background-color: $bgColor;
}

");

            Assert.Equal("div { background-color: purple; }", sheet.ToString());
        }

        [Fact]
        public void IfTest7()
        {
            var sheet = StyleSheet.Parse(@"div { color: if(red == red, purple, green) }");

            Assert.Equal("div { color: purple; }", sheet.ToString());
        }

        [Fact]
        public void IfTest8()
        {
            var sheet = StyleSheet.Parse(@"div { color: if(red == orange, purple, green) }");

            Assert.Equal("div { color: green; }", sheet.ToString());
        }


        [Fact]
        public void IfTest4()
        {
            var sheet = StyleSheet.Parse(@"
$bgColor: #ffffff;

@if rgba($bgColor, 0.5) == rgba(255, 255, 255, 0.5) { 
  div { color: red; display: none; }
  div { background-color: orange; }
}
");


            Assert.Equal(
@"div {
  color: red;
  display: none;
}
div { background-color: orange; }", sheet.ToString());
        }
    }
}
