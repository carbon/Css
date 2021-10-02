using Carbon.Color;

namespace Carbon.Css.Gradients;

public readonly struct AngularColorStop
{
    public AngularColorStop(Rgba32 color, double position, double angle = 0)
    {
        Color = color;
        Position = position;
        Angle = angle;
    }

    public readonly Rgba32 Color { get; }

    public readonly double Position { get; }

    public readonly double Angle { get; }
}