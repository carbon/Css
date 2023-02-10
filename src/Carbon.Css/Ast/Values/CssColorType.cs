namespace Carbon.Css;

internal enum CssColorType : byte
{
                     //             | Chrome | Safari | Firefox
    Rgb         = 1,  // rgb         | 
    Hsl         = 2,  // hsl            
    Hwb         = 3,  // hwb             
    Lab         = 4,  // lab         | 111+  | 15+    |
    Lch         = 5,  // lch         | 111+  | 15+    |
    OkLab       = 6,  // oklab       |
    Srgb        = 7,  //
    SrgbLinear  = 8,  // srgb-linear |
    DisplayP3   = 10, // display-p3
    A98Rgb      = 11, // a98-rgb
    ProPhotoRgb = 12, // prophoto-rgb
    Rec2020     = 13, // rec2020
    Xyz         = 15, // xyz
    XyzD50      = 16, // xyz-d65
    XyzD65      = 17, // xyz-d65
}


// <predefined-rgb> = 
//   srgb          |
//   srgb-linear   |
//   display-p3    |
//   a98-rgb       |
//   prophoto-rgb  |
//   rec2020


// Safari...
// A98RGB,
// DisplayP3,
// LCH,
// Lab,
// LinearSRGB,
// ProPhotoRGB,
// Rec2020,
// SRGB,
// XYZ_D50,
// XYZ_D65,

// https://github.com/WebKit/WebKit/blob/54fe2cbb5eddd356eea81e08228e470d49a83f94/Source/WebCore/platform/graphics/ColorTypes.h