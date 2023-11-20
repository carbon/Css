using System.Diagnostics.CodeAnalysis;

namespace Carbon.Css.Helpers;

internal static class ThrowHelper
{
    [DoesNotReturn]
    public static void RecursionDetected()
    {
        throw new Exception("Recursion detected");
    }

    [DoesNotReturn]
    public static void SelfReferencing()
    {
        throw new Exception("Self referencing");
    }
}