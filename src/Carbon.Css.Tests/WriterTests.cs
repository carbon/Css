using System.IO;
using System.Text;

namespace Carbon.Css.Tests;

public class WriterTests
{
    [Fact]
    public void WriteStyle()
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