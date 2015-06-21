namespace Carbon.Css.Tests
{
	using Xunit;

	public class VariableTests
	{
		[Fact]
		public void ParseVariables()
		{
			var text = @"$color: red;
			
			";

			Assert.Equal(1, StyleSheet.Parse(text).Context.Scope.Count);
		}

		[Fact]
		public void VariableTest1()
		{
			var sheet = StyleSheet.Parse(
@"
$blue: #dceef7;
$yellow: #fff5cc;

body { 
  background-color: $blue;
  color: $yellow;
}
");
			Assert.Equal("#dceef7", sheet.Context.Scope["blue"].ToString());

			Assert.Equal(2, sheet.Context.Scope.Count);

			Assert.Equal(
@"body {
  background-color: #dceef7;
  color: #fff5cc;
}", sheet.ToString());


		}

		[Fact]
		public void VariableTest3()
		{
			var context = new CssContext();

			context.Scope["monster"] = CssValue.Parse("red");

			var sheet = StyleSheet.Parse(
@"
$blue: #dceef7;
$yellow: #fff5cc;

body { 
  background-color: $blue;
  color: $yellow;
  monster: $monster;
}
", context);



			Assert.Equal(
@"body {
  background-color: #dceef7;
  color: #fff5cc;
  monster: red;
}", sheet.ToString());
		}


		[Fact]
		public void VariableTest4()
		{
			var context = new CssContext();

			context.Scope["monster"] = CssValue.Parse("purple");

			var sheet = StyleSheet.Parse(
@"
$blue: #dceef7;
$yellow: #fff5cc;
$padding: 10px;
$padding-right: 20px;
body { 
  background-color: $blue;
  color: $yellow;
  monster: $monster;
  padding: $padding $padding-right $padding $padding;
}
", context);



			Assert.Equal(
@"body {
  background-color: #dceef7;
  color: #fff5cc;
  monster: purple;
  padding: 10px 20px 10px 10px;
}", sheet.ToString());
		}


		[Fact]
		public void VariableTest2()
		{
			var styles =
@"
$addYellow: #fff5cc;
$editBlue: #dceef7;

body { font-size: 14px; opacity: 0.5; }
.editBlock button.save { background: $addYellow; }
.editBlock.populated button.save { background: $editBlue; }
.rotatedBox { box-sizing: border-box; }
			";

			var sheet = StyleSheet.Parse(styles);


			Assert.Equal(
@"body {
  font-size: 14px;
  opacity: 0.5;
}
.editBlock button.save { background: #fff5cc; }
.editBlock.populated button.save { background: #dceef7; }
.rotatedBox { box-sizing: border-box; }", sheet.ToString());


		}
	}
}
