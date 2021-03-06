﻿using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Carbon.Css.Json
{
    internal sealed class ThinknessJsonConverter : JsonConverter<Thickness>
    {
        public override Thickness Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return Thickness.Parse(reader.GetString()!);
        }

        public override void Write(Utf8JsonWriter writer, Thickness value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
