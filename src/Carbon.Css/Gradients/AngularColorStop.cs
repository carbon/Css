using System.Text.Json.Serialization;

using Carbon.Color;

namespace Carbon.Css.Gradients;

public readonly struct AngularColorStop
{
    [JsonConstructor]
    public AngularColorStop(Rgba32 color, double position, double angle = 0)
    {
        Color = color;
        Position = position;
        Angle = angle;
    }

    [JsonPropertyName("color")]
    public Rgba32 Color { get; }

    [JsonPropertyName("position")]
    public double Position { get; }

    [JsonPropertyName("angle")]
    public double Angle { get; }
}