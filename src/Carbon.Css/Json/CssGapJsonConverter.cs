using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Carbon.Css.Json;

public sealed class CssGapJsonConverter : JsonConverter<CssGap>
{
    public override CssGap Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return CssGap.Parse(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, CssGap value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}