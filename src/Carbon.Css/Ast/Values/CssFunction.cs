using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Carbon.Css;

public class CssFunction(string name, CssValue arguments) 
    : CssValue(NodeKind.Function)
{
    public string Name { get; } = name;

    public CssValue Arguments { get; } = arguments;

    public override CssFunction CloneNode() => new(Name, Arguments);

    internal override void WriteTo(TextWriter writer)
    {
        writer.Write(Name);
        writer.Write('(');
        Arguments.WriteTo(writer);
        writer.Write(')');
    }

    internal override void WriteTo(scoped ref ValueStringBuilder sb)
    {
        sb.Append(Name);
        sb.Append('(');
        Arguments.WriteTo(ref sb);
        sb.Append(')');
    }

    [SkipLocalsInit]
    public override string ToString()
    {
        var sb = new ValueStringBuilder(stackalloc char[128]);

        WriteTo(ref sb);

        return sb.ToString();
    }
}