using System.Text;
using System.Text.Json;

namespace Carbon.Css.Tests;

public class CssGapTests
{
    [Fact]
    public void Construct()
    {
        var gap = new CssGap(CssUnitValue.Parse("10%"));

        Assert.Equal("10%", gap.X.ToString());
        Assert.Equal("10%", gap.Y.ToString());
    }

    [Fact]
    public void CanParseStack()
    {
        Span<char> buffer = stackalloc char[3] { '1', '0', '%' };

        var gap = new CssGap(CssUnitValue.Parse(buffer));

        Assert.Equal("10%", gap.X.ToString());
        Assert.Equal("10%", gap.Y.ToString());
    }

    [Fact]
    public void ParseSingleValue()
    {
        var gap = CssGap.Parse("10px");

        Assert.Equal("10px", gap.X.ToString());
        Assert.Equal("10px", gap.Y.ToString());
    }

    [Theory]
    [InlineData("10px")]
    [InlineData("1em 2em")]
    [InlineData("20% 30%")]
    public void CanRoundtrip(string text)
    {
        var json = $"\"{text}\"";

        Assert.Equal(json, JsonSerializer.Serialize(CssGap.Parse(text)));
        Assert.Equal(json, JsonSerializer.Serialize(CssGap.Parse(Encoding.UTF8.GetBytes(text))));

        var deserialized = JsonSerializer.Deserialize<CssGap>(json);

        Assert.Equal(text, deserialized.ToString());
        Assert.Equal(CssGap.Parse(text), deserialized);

    }

    [Fact]
    public void ParseSingleValueUtf8()
    {
        var gap = CssGap.Parse("10px"u8);

        Assert.Equal("10px", gap.X.ToString());
        Assert.Equal("10px", gap.Y.ToString());
    }

    [Fact]
    public void ParseDecimalValue()
    {
        var gap = CssGap.Parse("10.5rem");

        Assert.Equal("10.5rem", gap.X.ToString());
        Assert.Equal("10.5rem", gap.Y.ToString());
    }

    [Fact]
    public void ParseDoubleValue()
    {
        var gap = CssGap.Parse("10px 20px");

        Assert.Equal("10px", gap.X.ToString());
        Assert.Equal("20px", gap.Y.ToString());
    }

    [Fact]
    public void ParseDoubleValueUtf8()
    {
        var gap = CssGap.Parse("10px 20px"u8);

        Assert.Equal("10px", gap.X.ToString());
        Assert.Equal("20px", gap.Y.ToString());
    }
}