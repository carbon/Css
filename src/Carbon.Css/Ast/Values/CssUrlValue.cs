using System.IO;
using System.Text;

namespace Carbon.Css;

public readonly struct CssUrlValue
{
    // url('')

    public CssUrlValue(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        Value = value;
    }

    public CssUrlValue(byte[] data, string contentType)
    {
        ArgumentNullException.ThrowIfNull(data);

        var sb = new ValueStringBuilder(12 + contentType.Length + (data.Length * 2));

        sb.Append("data:");
        sb.Append(contentType);
        sb.Append(";base64,");
        sb.Append(Convert.ToBase64String(data));

        Value = sb.ToString();
    }

    public string Value { get; }

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
        if (Value.Length is 0) return null!;

        if (!IsPath)
        {
            throw new ArgumentException(string.Concat("Has scheme: ", Value.AsSpan(0, Value.IndexOf(':'))));
        }

        // Already absolute
        if (Value[0] == '/')
        {
            return Value.ToString();
        }

        if (basePath[0] == '/')
        {
            basePath = basePath[1..];
        }

        // TODO: Eliminate this allocation

        // https://dev/styles/
        var baseUri = new Uri("https://dev/" + basePath);

        // Absolute path
        return new Uri(baseUri, relativeUri: Value).AbsolutePath;
    }

    private static readonly char[] trimChars = { '\'', '\"', '(', ')' };

    public static CssUrlValue Parse(ReadOnlySpan<char> text)
    {
        if (text.StartsWith("url", StringComparison.Ordinal))
        {
            text = text.Slice(3);
        }

        if (text[0] is '(' or '"' or '\'')
        {
            text = text.Trim(trimChars);
        }

        return new CssUrlValue(text.ToString());
    }
}
