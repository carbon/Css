namespace Carbon.Css;

public sealed class CharsetRule(ReadOnlyMemory<char> text) : CssRule
{
    public override RuleType Type => RuleType.Charset;

    public ReadOnlyMemory<char> Encoding { get; } = text;
}

// @charset "UTF-8";

// https://www.iana.org/assignments/character-sets/character-sets.xhtml