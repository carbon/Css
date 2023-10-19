using System.Text.Json.Serialization;

namespace Carbon.Css;

[JsonConverter(typeof(JsonStringEnumConverter<CssVisibility>))]
public enum CssVisibility : byte
{
    Visible  = 1,
    Hidden   = 2,
    Collapse = 3
}