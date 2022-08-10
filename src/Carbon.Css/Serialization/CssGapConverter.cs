using System.Buffers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Carbon.Css.Serialization;

public sealed class CssGapConverter : JsonConverter<CssGap>
{
    public override CssGap Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return CssGap.Parse(reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan);
    }

    public override void Write(Utf8JsonWriter writer, CssGap value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}