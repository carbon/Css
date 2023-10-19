using System.Text.Json.Serialization;

namespace Carbon.Css;

[JsonConverter(typeof(JsonStringEnumConverter<CssTextTransform>))]
public enum CssTextTransform
{
    None         = 0,
    Capitalize   = 1,
    LowerCase    = 2,
    UpperCase    = 3,
    FullWidth    = 4,
    FullSizeKana = 5
}