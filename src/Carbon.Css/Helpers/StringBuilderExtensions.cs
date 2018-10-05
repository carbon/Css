using System.Text;

namespace Carbon.Css
{
    internal static class StringBuilderExtensions
    {
        public static string Extract(this StringBuilder sb)
        {
            var value = sb.ToString();

            sb.Clear();

            return value;
        }
    }
}