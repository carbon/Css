using System.Text.Json.Serialization;

namespace Carbon.Css.Ast.Values;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CssNumericType
{
    Length = 1,
    Angle = 2,
    Time = 3,
    Frequency = 4,
    Resolution = 5,
    Flex = 6,
    Percent = 7
}