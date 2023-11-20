using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Carbon.Css.Serialization;

public sealed class ThicknessJsonConverter : JsonConverter<Thickness>
{
    [SkipLocalsInit]
    public override Thickness Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        int length = reader.HasValueSequence
            ? checked((int)reader.ValueSequence.Length)
            : reader.ValueSpan.Length;

        scoped Span<char> buffer = length <= 32
            ? stackalloc char[32]
            : new char[length];

        ReadOnlySpan<char> text = buffer.Slice(0, reader.CopyString(buffer));

        return Thickness.Parse(text);
    }

    public override void Write(Utf8JsonWriter writer, Thickness value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}