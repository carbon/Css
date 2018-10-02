using System;
using System.Text;

namespace Carbon.Css
{
    public readonly struct CssUrlValue
    {
        // url('')

        public CssUrlValue(string value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public CssUrlValue(byte[] data, string contentType)
        {
            // Works for resources only up to 32k in size in IE8.

            var sb = new StringBuilder();

            sb.Append("data:");
            sb.Append(contentType);
            sb.Append(";base64,");
            sb.Append(Convert.ToBase64String(data));
            
            Value = sb.ToString();
        }

        public string Value { get; }

        public override string ToString() => $"url('{Value}')";

        #region Helpers
        
        public bool IsPath => Value.IndexOf(':') == -1; // ! https://

        public bool IsExternal => !IsPath;

        public string GetAbsolutePath(string basePath) /* /styles/ */
        {
            if (!IsPath)
            {
                throw new ArgumentException("Has scheme:" + Value.Split(Seperators.Colon)[0]);
            }

            // Already absolute
            if (Value.Length > 0 && Value[0] == '/')
            {
                return Value;
            }

            if (basePath[0] == '/')
            {
                basePath = basePath.Substring(1);
            }

            // http://dev/styles/
            var baseUri = new Uri("http://dev/" + basePath);

            // Absolute path
            return new Uri(baseUri, relativeUri: Value).AbsolutePath;
        }

        #endregion

        private static readonly char[] trimChars = { '\'', '\"', '(', ')' };

        public static CssUrlValue Parse(string text)
        {
            var value = text.Replace("url", string.Empty).Trim(trimChars);

            return new CssUrlValue(value);
        }
    }
}