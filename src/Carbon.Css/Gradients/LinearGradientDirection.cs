using System.Text.Json.Serialization;

namespace Carbon.Css.Gradients;

[JsonConverter(typeof(JsonStringEnumConverter<LinearGradientDirection>))]
public enum LinearGradientDirection
{
    None        = 0,
                    
    Top         = 1, // to top      | 0deg
    Bottom      = 2, // to bottom   | 180deg
    Left        = 3, // to left     | 270deg
    Right       = 4, // to right    | 90deg

    // Specify the corner the line goes toward
    // The angle is calculated on the aspect ratio of the containing box
    TopLeft     = 5,
    TopRight    = 6,
    BottomLeft  = 7,
    BottomRight = 8
}

// (top | bottom) (left | right)