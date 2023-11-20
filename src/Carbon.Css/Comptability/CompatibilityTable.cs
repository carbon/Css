using System.Numerics;
using System.Runtime.CompilerServices;

namespace Carbon.Css;

public readonly struct CompatibilityTable(
    float chrome = 0,
    float edge = 0,
    float firefox = 0,
    float safari = 0)
{
    public float Chrome { get; } = chrome;

    public float Edge { get; } = edge;

    public float Firefox { get; } = firefox;

    public float Safari { get; } = safari;

    public bool IsDefined => Unsafe.BitCast<CompatibilityTable, Vector4>(this) != Vector4.Zero; //  Chrome > 0 || Firefox > 0 || Edge > 0 || Safari > 0;
}