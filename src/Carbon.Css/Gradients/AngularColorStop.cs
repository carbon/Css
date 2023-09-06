using System.Text.Json.Serialization;

using Carbon.Color;

namespace Carbon.Css.Gradients;

[method: JsonConstructor]
public readonly struct AngularColorStop(Rgba32 color, double position, double angle = 0)
{
    [JsonPropertyName("color")]
    public Rgba32 Color { get; } = color;

    [JsonPropertyName("position")]
    public double Position { get; } = position;

    [JsonPropertyName("angle")]
    public double Angle { get; } = angle;
}