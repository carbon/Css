#if NETSTANDARD2_0
namespace System
{
    internal static class StringExtensions
    {
        public static bool Contains(this string text, char c)
        {
            return text.IndexOf(c) > -1;
        }
    }
}
#endif