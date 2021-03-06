﻿using System;
using System.IO;
using System.Text;

namespace Carbon.Css
{
    public readonly struct CssUrlValue
    {
        // url('')

        public CssUrlValue(string value)
        {
            Value = value;
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

        public readonly string Value { get; }

        public void WriteTo(TextWriter writer)
        {
            writer.Write("url('");
            writer.Write(Value);
            writer.Write("')");
        }

        public readonly override string ToString()
        {
            var sb = new StringWriter();

            WriteTo(sb);

            return sb.ToString();
        }

        public readonly bool IsPath => !Value.Contains(':'); // ! https://

        public readonly bool IsExternal => !IsPath;

        public readonly string GetAbsolutePath(string basePath) /* /styles/ */
        {
            if (!IsPath)
            {
                throw new ArgumentException("Has scheme:" + Value.Substring(0, Value.IndexOf(':')));
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

            // TODO: Eliminate this allocation

            // http://dev/styles/
            var baseUri = new Uri("http://dev/" + basePath);

            // Absolute path
            return new Uri(baseUri, relativeUri: Value).AbsolutePath;
        }

        private static readonly char[] trimChars = { '\'', '\"', '(', ')' };

        public static CssUrlValue Parse(ReadOnlySpan<char> text)
        {
            if (text.Length > 3 && text[0] == 'u' && text[1] == 'r' && text[2] == 'l') // url
            {
                text = text.Slice(3);
            }

            if (text[0] == '(' || text[0] == '"' || text[0] == '\'')
            {
                text = text.Trim(trimChars);
            }

            return new CssUrlValue(text.ToString());
        }
    }
}