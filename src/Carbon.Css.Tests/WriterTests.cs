namespace Carbon.Css.Tests
{
	using System.IO;
	using System.Text;
	using Xunit;

	public class WriterTests : FixtureBase
	{
		[Fact]
		public void StyleRule1()
		{
			var pieceStyle = new StyleRule("#piece_1") {
				{ "max-width", "960px" }
			};	

			var sb = new StringBuilder();

			using (var output = new StringWriter(sb))
			{
				pieceStyle.WriteTo(output);
			}

			Assert.Equal("#piece_1 { max-width: 960px; }", sb.ToString());
        }
	}
}


