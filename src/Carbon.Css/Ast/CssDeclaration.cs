using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace Carbon.Css;

public sealed class CssDeclaration : CssNode
{
    public CssDeclaration(string name, string value, string? priority = null)
        : this(name, CssValue.Parse(value), priority)
    { }

    public CssDeclaration(string name, CssValue value, string? priority = null)
        : base(NodeKind.Declaration)
    {

        Value = value;
        Priority = priority;
        Info = CssProperty.Get(name);
    }

    public CssDeclaration(string name, CssValue value, NodeKind kind)
        : base(kind)
    {
        Info = CssProperty.Get(name);
        Value = value;
    }

    public CssDeclaration(CssProperty property, CssValue value, string? priority)
        : base(NodeKind.Declaration)
    {
        Info = property;
        Value = value;
        Priority = priority;
    }

    public string Name => Info.Name;

    public CssValue Value { get; }

    public CssProperty Info { get; }

    public string? Priority { get; }

    public override CssDeclaration CloneNode() => new(Info, (CssValue)Value.CloneNode(), Priority);

    public void WriteTo(StringBuilder sb)
    {
        sb.Append(Info.Name);
        sb.Append(": ");
        Value.WriteTo(sb);

        if (Priority is not null)
        {
            sb.Append(" !");
            sb.Append(Priority);
        }
    }

    [SkipLocalsInit]
    public override string ToString()
    {
        // color: red !important

        var sb = new ValueStringBuilder(stackalloc char[32]);

        sb.Append(Info.Name);

        sb.Append(": ");

        if (Value is ISpanFormattable unitValue)
        {
            sb.AppendSpanFormattable(unitValue);
        }
        else
        {
            sb.Append(Value.ToString());
        }

        if (Priority is not null)
        {
            sb.Append(" !");
            sb.Append(Priority);
        }

        return sb.ToString();
    }
}