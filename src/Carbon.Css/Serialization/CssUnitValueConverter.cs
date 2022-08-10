using System.Buffers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Carbon.Css.Serialization;

public sealed class CssUnitValueConverter : JsonConverter<CssUnitValue>
{
    public override CssUnitValue Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return CssUnitValue.Parse(reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan);
    }

    public override void Write(Utf8JsonWriter writer, CssUnitValue value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}