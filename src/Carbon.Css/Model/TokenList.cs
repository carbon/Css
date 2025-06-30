using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Carbon.Css;

using Parser;

public sealed class TokenList : List<CssToken>
{
    [SkipLocalsInit]
    public override string ToString()
    {
        if (Count is 1)
        {
            return this[0].IsTrivia ? string.Empty : this[0].Text;
        }
        else
        {
            var sb = new ValueStringBuilder(stackalloc char[64]);

            WriteTo(ref sb);

            return sb.ToString();
        }
    }

    private void WriteTo(scoped ref ValueStringBuilder writer)
    {
        var span = CollectionsMarshal.AsSpan(this);

        for (int i = 0; i < Count; i++)
        {
            ref readonly CssToken token = ref span[i];

            if (token.IsTrivia)
            {
                // Skip leading and trailing trivia
                if (i is 0 || i + 1 == span.Length) continue;

                writer.Append(' ');

                continue;
            }

            writer.Append(token.Text);
        }
    }

    public void WriteTo(TextWriter writer, CssScope scope)
    {
        var span = CollectionsMarshal.AsSpan(this);

        for (int i = 0; i < span.Length; i++)
        {
            ref readonly CssToken token = ref span[i];
            bool isEnd = i + 1 == span.Length;

            if (token.Kind is CssTokenKind.Dollar && !isEnd)
            {
                string variableName = this[++i].Text;

                if (scope.TryGetValue(variableName, out var value))
                {
                    value.WriteTo(writer);
                }
                else
                {
                    throw new Exception($"{variableName} not found");
                }

                continue;
            }

            if (token.IsTrivia)
            {
                // Skip leading and trailing trivia
                if (i is 0 || isEnd) continue;

                writer.Write(' ');

                continue;
            }

            writer.Write(token.Text);
        }        
    }

    public void WriteTo(TextWriter writer)
    {
        if (Count is 1)
        {
            if (this[0].IsTrivia) return;

            writer.Write(this[0].Text);

            return;
        }

        for (int i = 0; i < Count; i++)
        {
            CssToken token = this[i];

            if (token.IsTrivia)
            {
                // Skip leading and trailing trivia
                if (i is 0 || i + 1 == Count) continue;

                writer.Write(' ');

                continue;
            }

            writer.Write(token.Text);
        }
    }
}