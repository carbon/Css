using Carbon.Color;

namespace Carbon.Css.Gradients
{
    public readonly struct AngularColorStop
    {
        public AngularColorStop(Rgba32 color, double position, double angle = 0)
        {
            Color = color;
            Position = position;
            Angle = angle;
        }

        public Rgba32 Color { get; }

        public double Position { get; }

        public double Angle { get; }
    }
}