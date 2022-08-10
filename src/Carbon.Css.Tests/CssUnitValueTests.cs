using System.Text;
using System.Text.Json;

namespace Carbon.Css.Tests;

public class CssUnitValueTests
{
    [Fact]
    public void CanRoundtrip()
    {
        var value = new CssUnitValue(100, CssUnitInfo.Px);

        Assert.Equal("\"100px\"", JsonSerializer.Serialize(value));
        Assert.Equal(value, JsonSerializer.Deserialize<CssUnitValue>("\"100px\""));
    }

    [Theory]
    [InlineData("1px",           1d,   "px")]
    [InlineData("100em",         100d, "em")]
    [InlineData("0.5rem",        0.5d, "rem")]
    [InlineData("1000000000deg", 1000000000d, "deg")]
    public void ParseUtf8(string text, double value, string unit)
    {
        var result = CssUnitValue.Parse(Encoding.UTF8.GetBytes(text));

        Assert.Equal(value, result.Value);
        Assert.Equal(unit, result.Unit.Name);

        var json = $"\"{text}\"";

        Assert.Equal(json,   JsonSerializer.Serialize(result));
        Assert.Equal(result, JsonSerializer.Deserialize<CssUnitValue>(json));
        Assert.Equal(result, CssUnitValue.Parse(text));
    }

    [Fact]
    public void ParseUtf8_Space()
    {
        var result = CssUnitValue.Parse("1 px"u8);

        Assert.Equal(1, result.Value);
        Assert.Same(CssUnitInfo.Px, result.Unit);
    }

    [Fact]
    public void ParseUtf_Unitless()
    {
        var result = CssUnitValue.Parse("-1234567890"u8);

        Assert.Equal(-1234567890, result.Value);
        Assert.Same(CssUnitInfo.Number, result.Unit);
    }

    [Fact]
    public void ParseUtf_NoDecimal()
    {
        var result = CssUnitValue.Parse("1234567890.0123456789"u8);

        Assert.Equal(1234567890.0123456789d, result.Value);
        Assert.Same(CssUnitInfo.Number, result.Unit);
    }
}
