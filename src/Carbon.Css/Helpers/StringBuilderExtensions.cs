using System.Text;

namespace Carbon.Css
{
    internal static class StringBuilderExtensions
    {
        /// <summary>
        /// Extracts the buffered value and clears the string builder.
        /// </summary>
        public static string Extract(this StringBuilder sb)
        {
            var value = sb.ToString();

            sb.Clear();

            return value;
        }
    }
}
