using System.Text;

namespace Carbon.Css;

public sealed class CssBoolean(bool value) : CssValue(NodeKind.Boolean)
{
    public static readonly CssBoolean True  = new(true);
    public static readonly CssBoolean False = new(false);

    public bool Value { get; } = value;

    public override CssBoolean CloneNode() => new(Value);

    public override string ToString() => Value ? "true" : "false";

    internal override void WriteTo(scoped ref ValueStringBuilder sb)
    {
        sb.Append(Value ? "true" : "false");
    }

    internal static CssBoolean Get(bool value)
    {
        return value ? True : False;
    }
}