using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Carbon.Css.Serialization;

public sealed class CssGapConverter : JsonConverter<CssGap>
{
    [SkipLocalsInit]
    public override CssGap Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        int length = reader.HasValueSequence
            ? checked((int)reader.ValueSequence.Length)
            : reader.ValueSpan.Length;

        scoped Span<byte> buffer = length <= 32
            ? stackalloc byte[32]
            : new byte[length];

        ReadOnlySpan<byte> text = buffer.Slice(0, reader.CopyString(buffer));

        return CssGap.Parse(text);
    }

    public override void Write(Utf8JsonWriter writer, CssGap value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}